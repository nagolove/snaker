using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Snake : MonoBehaviour
{
    GameObject head, circle;
    float spriteSize;
    Vector2 ds;
    public float speed = 10;
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

    void setupSpriteSize()
    {
        SpriteRenderer spr = head.GetComponent<SpriteRenderer>();
        Vector3 size = (spr.bounds.max - spr.bounds.min);
        spriteSize = size.x;
    }
    void Start()
    {
        head = GameObject.Find("head");
        circle = GameObject.Find("circle");

        setupSpriteSize();

        // putTextAtPoint(new Vector2(transform.position.x, transform.position.y));
    }

    /*
    Тут какой-то умный поиск позиции. Возвращает локальный вектор относительно данного.
    */
    Vector3 findNewPosition(Vector3 point)
    {
        Vector3 p;
        do
        {
            float x = UnityEngine.Random.Range(-spriteSize, spriteSize);
            float y = UnityEngine.Random.Range(-spriteSize, spriteSize);
            p = new Vector3(x, y, 0);
        }
        while ((point - p).magnitude > spriteSize);
        return p;
    }
    void AddNode()
    {
        GameObject o = null;
        Collider2D[] results = new Collider2D[3];
        int collidersNum = 0;
        int maxAttemps = 10;

        while (true)
        {
            Vector3 pos = findNewPosition(transform.position);
            Debug.Log(String.Format("pos` {0}, {1}", pos.x, pos.y));
            o = Instantiate(circle, pos, Quaternion.identity);
            Collider2D collider = o.GetComponent<CircleCollider2D>();
            collidersNum = collider.OverlapCollider(new ContactFilter2D(), results);
            if (collidersNum != 0)
                Destroy(o);
            if (collidersNum == 0 || maxAttemps <= 0)
                break;
            maxAttemps--;
        }
        Debug.Log(String.Format("max attempts {0}, collidersNum {1}", maxAttemps, collidersNum));
        if (collidersNum != 0)
        {
            Destroy(o);
        }
        else
        {
            o.layer = 0; //ставлю дефолтное значение, делаю видимым
            nodes.Add(o);
        }
    }

    void CheckCollisionPoints()
    {
        Collider2D collider = GetComponent<CircleCollider2D>();
        ContactPoint2D[] results = new ContactPoint2D[10];
        // int pointsNum = collider.GetContacts(new ContactFilter2D(), results);
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

        if (Input.GetKeyDown("a"))
        {
            Debug.Log("add node");
            AddNode();
        }
        if (Input.GetKeyDown("s"))
        {
            CheckCollisionPoints();
        }

        Vector3 ds3 = new Vector3(ds.x, ds.y, 0);
        head.transform.position += ds3;

        ds *= 0.8f;

        // GameObject prev = head;
        Vector3 prev = head.transform.position;
        Vector3 t;
        /*
        Смещаю текущий элемент на позицию предыдущего. Двигаюсь от следующего за головой(первый в списке) к хвосту.
        */
        foreach (GameObject o in nodes)
        {
            t = o.transform.position;
            o.transform.position = prev;
            prev = t;
        }
    }
}
