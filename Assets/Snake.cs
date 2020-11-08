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

    Vector3 findNewPosition(float r)
    {
        Common.Polar polar;
        polar.angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2.0f);
        polar.length = (r + spriteSize) / 2.0f;
        Vector2 cart = Common.fromPolar(polar);
        return new Vector3(cart.x, cart.y, 0);
    }
    void AddNode(Vector3 pos)
    {
        GameObject o = null;
        Collider2D[] results = new Collider2D[3];
        int collidersNum = 0;
        int maxAttemps = 20;
        float r = getSpriteSize(circle);
        while (true)
        {
            o = Instantiate(circle, pos + findNewPosition(r + 0.1f), Quaternion.identity);
            Collider2D collider = o.GetComponent<CircleCollider2D>();
            collidersNum = collider.OverlapCollider(new ContactFilter2D(), results);
            if (collidersNum != 0)
                Destroy(o);
            if (collidersNum == 0 || maxAttemps <= 0)
                break;
            maxAttemps--;
        }
        // Debug.Log(String.Format("max attempts {0}, collidersNum {1}", maxAttemps, collidersNum));
        if (collidersNum != 0)
        {
            // Debug.Log("Destroy");
            Destroy(o);
        }
        else
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
                if (nodes.Count > 0)
                {
                    pos = nodes.Last().transform.position;
                }
                AddNode(pos);
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
            t = o.transform.position;
            // o.transform.position = prev;
            prev = t;
        }
    }
}
