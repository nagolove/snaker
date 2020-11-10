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
    Vector3 ds, dsOrig;
    public float speed = 2.0f;
    public float q = 3;
    public static int num;
    public float rotAngle = 2.0f;
    List<GameObject> nodes = new List<GameObject>();
    Vector3 lastPosition;
    GameObject leftEye, rightEye;

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
        k = - Vector2.Perpendicular(k).normalized;
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
    void Update()
    {
        lastPosition = transform.position;
        Vector3 dir = getEyeDirection();

        if (Input.GetKey("left"))
            transform.Rotate(0, 0, -rotAngle);
        if (Input.GetKey("right"))
            transform.Rotate(0, 0, rotAngle);
        
        // cделать нормальное ускорение
        // if (Input.GetKey("up"))
            // dir *= 1.1f;

        // следать торможение
        /*
        if (Input.GetKey("down"))
            ds = new Vector2(0, -speed * Time.deltaTime);
        */

        if (!Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown("a"))
            {
                Grow();
            }
        }

        // dsOrig = dir;
        head.transform.position += dir * speed * Time.deltaTime;
        // ds *= 0.8f;

        MoveTail();
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
        float dsLen = 1.0f;
        if (lastPosition - transform.position != Vector3.zero) // может быть неточное сравнение плавающих чисел
        {
            // PushDrawLine(transform.position, transform.position + diff, Color.red);
            foreach (GameObject o in nodes)
            {
                Vector3 t = o.transform.position;
                Vector3 dir = prev - o.transform.position;
                dir = Vector3.ClampMagnitude(dir, dsLen);
                // PushDrawLine(o.transform.position, o.transform.position + dir, Color.red);
                float len = (lastPosition - o.transform.position).magnitude;
                o.transform.position += dir;
                prev = t;
            }
        }
    }
}
