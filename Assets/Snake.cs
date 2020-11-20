using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;
using static UnityEngine.Random;
using static Common;

public class Snake : MonoBehaviour
{
    GameObject head, circle;
    float spriteSize;
    public float speed = 1.0f;
    public static int num;
    [Header("Угол поворота в градусах")]
    public float rotAngle;
    List<GameObject> nodes = new List<GameObject>();
    Vector3 lastPosition;
    GameObject leftEye, rightEye;
    Boolean speedUp;
    float speedUpTime;
    public float maxSpeedUp = 7; // во сколько раз вырастет скорость максимум
    public float speedUpAccelerationTime = 1.0f; // время разгона в секундах
    Vector3 delta;
    LineDrawer lineDrawer = new LineDrawer();
    CameraController camController;
    SnakesManager snakesManager;
    public bool userControlled = false;

    void OnRenderObject()
    {
        lineDrawer.DrawList();
    }
    Vector3 getEyeDirection()
    {
        Vector3 n = (rightEye.transform.position - leftEye.transform.position);
        Vector2 k = new Vector2(n.x, n.y);
        k = -Vector2.Perpendicular(k).normalized;
        lineDrawer.PushLine(transform.position, transform.position + (new Vector3(k.x, k.y)) * 5.0f, Color.red);
        return new Vector3(k.x, k.y);
    }
    void putTextAtPoint(Transform trans)
    {
        GameObject ngo = new GameObject("SnakeNumber");
        ngo.transform.Translate(trans.position);
        TextMeshPro t = ngo.AddComponent<TextMeshPro>();
        t.transform.SetParent(trans);
        t.fontSize = 8;
        t.text = String.Format("{0}", num++);
        t.alignment = TextAlignmentOptions.Center;
        t.color = Color.black;
    }
    void Start()
    {
        Debug.LogFormat("Start snake [{0}]", name);
        head = this.gameObject;
        circle = head.transform.Find("circle").gameObject;
        spriteSize = getSpriteSize(head);
        putTextAtPoint(transform);
        leftEye = head.transform.Find("eye_l").gameObject;
        rightEye = head.transform.Find("eye_r").gameObject;
        isAccelerated = false;
        camController = Camera.main.GetComponent<CameraController>();
        snakesManager = GameObject.Find("snakesManager").GetComponent<SnakesManager>();
        if (!snakesManager)
        {
            Debug.LogWarning("snakes manager not found.");
        }
    }
    public void rotateLeft()
    {
        transform.Rotate(0, 0, rotAngle);
    }

    public void rotateRight()
    {
        transform.Rotate(0, 0, -rotAngle);
    }

    public void setRotation(float angle)
    {
        transform.Rotate(0, 0, angle);
    }

    Vector3 checkAccelerate(Boolean buttonValue, Vector3 delta)
    {
        Vector3 newDelta = delta;
        if (buttonValue)
        {
            if (!speedUp)
            {
                //init
                speedUpTime = Time.realtimeSinceStartup;
                Debug.LogWarning(String.Format("speedUp at time {0}", speedUpTime));
                speedUp = true;
            }
            float t = Time.realtimeSinceStartup - speedUpTime;
            // коли не прошло одной секунды - интерполирую
            if (t < speedUpAccelerationTime)
                newDelta = Vector3.Lerp(delta, delta * maxSpeedUp, t);
            else
                newDelta *= maxSpeedUp;
        }
        else
            speedUp = false;
        return newDelta;
    }

    bool isAccelerated;
    public void accelerate()
    {
        isAccelerated = true;
    }
    void Update()
    {
        lastPosition = transform.position;
        delta = getEyeDirection() * speed * Time.deltaTime / 10.0f;
        delta = checkAccelerate(isAccelerated, delta);
        // сделать торможение

        if (userControlled)
        {
            camController.checkPointMovement(transform.position);
        }

        Vector3 newPos = head.transform.position + delta;
        if (snakesManager.inArea(newPos))
        {
            head.transform.position = newPos;
        }
        MoveTail();
        isAccelerated = false;
    }

    public void grow()
    {
        Vector3 pos = head.transform.position;
        float size = spriteSize;
        if (nodes.Count > 0)
        {
            GameObject obj = nodes.Last();
            pos = obj.transform.position;
            size = getSpriteSize(obj);
        }
        Debug.Log(String.Format("size {0}", size));
        AddNode(pos, size);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] points = new ContactPoint2D[collision.contactCount];
        int num = collision.GetContacts(points);
        foreach (ContactPoint2D point in points)
        {
            GameObject o = point.otherCollider.gameObject;
            o.transform.position -= new Vector3(point.relativeVelocity.x, point.relativeVelocity.y);
        }
    }
    void MoveTail()
    {
        Vector3 prev = transform.position;
        if (lastPosition - transform.position != Vector3.zero) // может быть неточное сравнение плавающих чисел
        {
            // PushDrawLine(transform.position, transform.position + diff, Color.red);
            foreach (GameObject o in nodes)
            {
                Vector3 t = o.transform.position;
                // PushDrawLine(o.transform.position, o.transform.position + dir, Color.red);
                o.transform.position += Vector3.ClampMagnitude(prev - o.transform.position, delta.magnitude);
                prev = t;
            }
        }
    }
    void AddNode(Vector3 pos, float size)
    {
        GameObject o = null;
        Collider2D[] results = new Collider2D[10];
        int collidersNum = 0;
        int maxAttemps = 50; // сколько попыток на окружность делать
        float r = getSpriteSize(circle);
        float angle = UnityEngine.Random.Range(0.0f, 2.0f * (float)Math.PI);
        float dAngle = (float)Math.PI * 2.0f / maxAttemps;
        for (int i = 0; i < maxAttemps; ++i)
        {
            Vector2 cart = Common.fromPolar(angle, ((r + 0.1f) + size) / 2.0f);
            angle += dAngle;
            o = Instantiate(circle, pos + new Vector3(cart.x, cart.y, 0), Quaternion.identity);
            Collider2D collider = o.GetComponent<CircleCollider2D>();
            collidersNum = collider.OverlapCollider(new ContactFilter2D(), results);
            if (collidersNum != 0)
            {
                Destroy(o);
                o = null;
            }
            else
                break;
        }
        if (o)
        {
            o.layer = 0; //ставлю дефолтное значение, делаю видимым
            putTextAtPoint(o.transform);
            SpriteRenderer renderer = o.GetComponent<SpriteRenderer>();
            renderer.color = new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), 1.0f);
            nodes.Add(o);
        }
    }

}
