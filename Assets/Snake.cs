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
    public float rotAngle = 2.0f;
    List<GameObject> nodes = new List<GameObject>();
    Vector3 lastPosition;
    GameObject leftEye, rightEye;
    Boolean speedUp;
    float speedUpTime;
    public float maxSpeedUp = 7; // во сколько раз вырастет скорость максимум
    public float speedUpAccelerationTime = 1.0f; // время разгона в секундах
    Vector3 delta;

    #region Lines drawing
    struct Line
    {
        public Vector2 from, to;
        public Color color;
        public Line(Vector2 from, Vector2 to, Color color)
        {
            this.from = from;
            this.to = to;
            this.color = color;
        }
    }
    List<Line> lines = new List<Line>();

    void PushDrawLine(Vector2 from, Vector2 to, Color color)
    {
        lines.Add(new Line(from, to, color));
    }
    void PushDrawLine(Vector3 from, Vector3 to, Color color)
    {
        lines.Add(new Line(from, to, color));
    }

    void DrawLineList()
    {
        foreach (Line line in lines)
        {
            Common.DrawLineGL(line.from, line.to, line.color);
        }
        lines.Clear();
    }

    void OnRenderObject()
    {
        DrawLineList();
    }
    #endregion
    Vector3 getEyeDirection()
    {
        Vector3 n = (rightEye.transform.position - leftEye.transform.position);
        Vector2 k = new Vector2(n.x, n.y);
        k = -Vector2.Perpendicular(k).normalized;
        PushDrawLine(transform.position, transform.position + (new Vector3(k.x, k.y)) * 5.0f, Color.red);
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
        head = GameObject.Find("SnakeHead");
        circle = head.transform.Find("circle").gameObject;
        // circle = GameObject.Find("circle");
        spriteSize = getSpriteSize(head);
        putTextAtPoint(transform);
        leftEye = head.transform.Find("eye_l").gameObject;
        rightEye = head.transform.Find("eye_r").gameObject;
    }
    void rotateLeft()
    {
        transform.Rotate(0, 0, rotAngle);
    }

    void rotateRight()
    {
        transform.Rotate(0, 0, -rotAngle);
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
    void Update()
    {
        lastPosition = transform.position;
        Vector3 dir = getEyeDirection();

        if (Input.GetKey("left")) rotateLeft();
        if (Input.GetKey("right")) rotateRight();

        delta = dir * speed * Time.deltaTime / 10.0f;
        delta = checkAccelerate(Input.GetKey("up"), delta);
        // сделать торможение
        /*
        if (Input.GetKey("down"))
            ds = new Vector2(0, -speed * Time.deltaTime);
        */
        checkHeadInCamera();

        if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown("a"))
        {
            Grow();
        }

        head.transform.position += delta;
        MoveTail();
    }

    void checkHeadInCamera()
    {
        Vector3 d = transform.position - Camera.main.transform.position;
        Vector2 d2 = new Vector2(d.x, d.y);
        float len = d2.magnitude;
        Camera cam = Camera.main;
        Debug.Log(String.Format("pixelRect x,y,w,h {0},{1},{2},{3}",
            cam.pixelRect.x, cam.pixelRect.y, cam.pixelRect.width, cam.pixelRect.height));
        Debug.Log(String.Format("rect x,y,w,h {0},{1},{2},{3}",
            cam.rect.x, cam.rect.y, cam.rect.width, cam.rect.height));
        Debug.Log(String.Format("scaled w*h {0},{1}", cam.scaledPixelWidth, cam.scaledPixelWidth));
        Debug.Log(String.Format("w*h {0},{1}", cam.pixelWidth, cam.pixelHeight));
        Debug.Log(String.Format("ortho {0}", cam.orthographicSize));
        Debug.Log(String.Format("scale {0}, {1}, {2}", cam.transform.localScale.x, cam.transform.localScale.y, cam.transform.localScale.z));
        Debug.Log(String.Format("w*h {0}, {0}", Screen.width, Screen.height));
        Debug.Log(String.Format("d {0}", d2.magnitude));
        
        Vector2 worldUnitsInCamera;
        worldUnitsInCamera.y = cam.orthographicSize * 2;
        worldUnitsInCamera.x = worldUnitsInCamera.y * Screen.width / Screen.height;

        Vector2 worldToPixelAmount;
        worldToPixelAmount.x = Screen.width / worldUnitsInCamera.x;
        worldToPixelAmount.y = Screen.height / worldUnitsInCamera.y;
        Debug.Log(String.Format("amount {0}, {1}", worldToPixelAmount.x, worldToPixelAmount.y));
        Vector2 pixelDist = worldToPixelAmount * len;
        Debug.Log(String.Format("pixelDist {0}, {1}", pixelDist.x, pixelDist.y));
    }

    void Grow()
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
        float angle = 0.0f;
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
