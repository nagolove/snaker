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
    public int num;
    List<GameObject> nodes = new List<GameObject>();
    Vector3 lastPosition;
    CircleDrawer circleDrawer;

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
        foreach(Line line in lines)
        {
            Common.DrawLineGL(line.from, line.to, line.color);
        }
        lines.Clear();
    }

    void OnRenderObject()
    {
        DrawLineList();
    }

    void putTextAtPoint(Vector2 p)
    {
        GameObject ngo = new GameObject("SnakeNumber");
        ngo.transform.Translate(new Vector3(p.x, p.y, 0));
        TextMeshPro t = ngo.AddComponent<TextMeshPro>();
        t.transform.SetParent(transform);
        t.fontSize = 8;
        t.text = String.Format("{0}", num);
        t.alignment = TextAlignmentOptions.Center;
        t.color = Color.black;
    }
    void Start()
    {
        head = GameObject.Find("head");
        circle = GameObject.Find("circle");

        spriteSize = getSpriteSize(head);
        circleDrawer = GetComponent<CircleDrawer>();
        // circleDrawer.
        // putTextAtPoint(new Vector2(transform.position.x, transform.position.y));
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
            Vector3 newPos = new Vector3(cart.x, cart.y, 0);
            angle += dAngle;
            o = Instantiate(circle, pos + newPos, Quaternion.identity);
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
            SpriteRenderer renderer = o.GetComponent<SpriteRenderer>();
            renderer.color = new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), 1.0f);
            nodes.Add(o);
        }
    }
    void CheckCollisionPoints()
    {
        Collider2D collider = GetComponent<CircleCollider2D>();
        ContactPoint2D[] results = new ContactPoint2D[10];
        Collider2D[] colliders = new Collider2D[10];
        int pointsNum = collider.OverlapCollider(new ContactFilter2D(), colliders);
        Debug.Log(String.Format("pointsNum {0}", pointsNum));
        foreach (Collider2D c in colliders)
        {
            Debug.Log(String.Format("collider {0}", c.name));
        }
        foreach (ContactPoint2D p in results)
        {
            Debug.Log(String.Format("point {0}, {1}", p.point.x, p.point.y));
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
                Debug.Log("add node");
                Vector3 pos = head.transform.position;
                float size = spriteSize;
                if (nodes.Count > 0)
                {
                    Debug.Log("from list");
                    GameObject obj = nodes.Last();
                    pos = obj.transform.position;
                    size = getSpriteSize(obj);
                }
                AddNode(pos, size);
            }
            if (Input.GetKeyDown("s"))
            {
                CheckCollisionPoints();
            }
        }

        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene("Main");
        }

        dsOrig = ds;
        head.transform.position += new Vector3(ds.x, ds.y, 0);
        ds *= 0.8f;

        MoveTail();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] points = new ContactPoint2D[collision.contactCount];
        int num = collision.GetContacts(points);
        foreach(ContactPoint2D point in points)
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
