using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;
using static UnityEngine.Random;

public class Snake : MonoBehaviour
{
    GameObject head, circle;
    float spriteSize;
    Vector2 ds;
    public float speed = 10;
    public float q = 3;
    public int num;
    List<GameObject> nodes = new List<GameObject>();
    Vector3 last;

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

    float getSpriteSize(GameObject obj)
    {
        SpriteRenderer spr = obj.GetComponent<SpriteRenderer>();
        return (spr.bounds.max - spr.bounds.min).x;
    }
    void Start()
    {
        head = GameObject.Find("head");
        circle = GameObject.Find("circle");

        spriteSize = getSpriteSize(head);

        // putTextAtPoint(new Vector2(transform.position.x, transform.position.y));
    }

    Vector3 findNewPosition(float len)
    {
        Common.Polar polar;
        polar.angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2.0f);
        polar.length = len;
        Vector2 cart = Common.fromPolar(polar);
        return new Vector3(cart.x, cart.y, 0);
    }
    void AddNode2(Vector3 pos, float size)
    {
        GameObject o = null;
        Collider2D[] results = new Collider2D[10];
        int collidersNum = 0;
        int maxAttemps = 100; // сколько попыток на окружность делать
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

            // Debug.Log(String.Format("collidersNum {0}", collidersNum));
            // for (int j = 0; j < collidersNum; ++j)
            // {
            //     Debug.Log(String.Format("res {0}", results[j].name));
            // }

            if (collidersNum != 0)
            {
                Destroy(o);
                o = null;
            }
            else
                break;
        }
        Debug.Log(String.Format("max attempts {0}, collidersNum {1}", maxAttemps, collidersNum));
        Debug.Log(String.Format("new pos {0}, {1}", o.transform.position.x, o.transform.position.y));
        if (o)
        {
            o.layer = 0; //ставлю дефолтное значение, делаю видимым
            SpriteRenderer renderer = o.GetComponent<SpriteRenderer>();
            renderer.color = new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), 1.0f);
            nodes.Add(o);
        }
    }
    void AddNode(Vector3 pos, float size)
    {
        GameObject o = null;
        Collider2D[] results = new Collider2D[3];
        int collidersNum = 0;
        int maxAttemps = 100; // сколько попыток на окружность делать
        float r = getSpriteSize(circle);
        while (true)
        {
            /*
            findNewPosition можно запускать с разным значением угла, проверяя по кругу подходящие места
            */
            o = Instantiate(circle, pos + findNewPosition(((r + 0.1f) + size) / 2.0f), Quaternion.identity);
            Collider2D collider = o.GetComponent<CircleCollider2D>();
            collidersNum = collider.OverlapCollider(new ContactFilter2D(), results);
            if (collidersNum != 0)
            {
                Destroy(o);
            }
            else
                break;
            if (maxAttemps-- <= 0)
                break;
        }
        Debug.Log(String.Format("max attempts {0}, collidersNum {1}", maxAttemps, collidersNum));
        Debug.Log(String.Format("new pos {0}, {1}", o.transform.position.x, o.transform.position.y));
        if (collidersNum != 0)
        {
            Debug.Log("Destroy");
            // Destroy(o);
        }
        // else
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
        last = transform.position;

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
                AddNode2(pos, size);
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

        Vector3 ds3 = new Vector3(ds.x, ds.y, 0);
        head.transform.position += ds3;

        ds *= 0.8f;

        // GameObject prev = head;
        // Vector3 prev = head.transform.position;
        Vector3 prev = last;
        Vector3 t;
        /*
        Смещаю текущий элемент на позицию предыдущего. Двигаюсь от следующего за головой(первый в списке) к хвосту.
        */
        foreach (GameObject o in nodes)
        {
            // t = o.transform.position;
            // o.transform.position = prev;
            // prev = t;
        }
    }
}
