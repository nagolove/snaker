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
    Vector2 ds, dsOrig;
    public float speed = 10;
    public float q = 3;
    public static int num;
    List<GameObject> nodes = new List<GameObject>();
    Vector3 lastPosition;

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

        if (Input.GetKey("left"))
            ds = new Vector2(-speed * Time.deltaTime, 0);
        if (Input.GetKey("right"))
            ds = new Vector2(speed * Time.deltaTime, 0);
        if (Input.GetKey("up"))
            ds = new Vector2(0, speed * Time.deltaTime);
        if (Input.GetKey("down"))
            ds = new Vector2(0, -speed * Time.deltaTime);

        if (!Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown("a"))
            {
                Grow();
            }
        }

        if (Input.GetKeyDown("r"))
        {
            // SceneManager.LoadScene("Main");
        }

        dsOrig = ds;
        head.transform.position += new Vector3(ds.x, ds.y, 0);
        ds *= 0.8f;

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
        float dsLen = dsOrig.magnitude;
        Vector3 diff = lastPosition - transform.position;
        float diffLen = diff.magnitude;
        if (diff != Vector3.zero) // может быть неточное сравнение плавающих чисел
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
