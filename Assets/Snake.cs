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
    Vector2 findNewPosition(Vector2 point)
    {
        float x = UnityEngine.Random.Range(-10, 10);
        float y = UnityEngine.Random.Range(-10, 10);
        return new Vector2(x, y);
    }
    void AddNode()
    {
        /*
        Сперва найти безопасную область вокруг последнего элемента. 
        Как найти такую область? Обработка столкновений и пересечений.
        Потом поместить туда новый объект.
        */
        Vector2 newPosition = findNewPosition(new Vector2()) * 2;
        Debug.Log(String.Format("newPosition {0}, {1}", newPosition.x, newPosition.y));
        Vector3 pos = transform.position;
        Debug.Log(String.Format("pos {0}, {1}", pos.x, pos.y));
        // Vector3 dir = last - transform.position;
        // pos = dir.normalized * 2;
        pos += new Vector3(newPosition.x, newPosition.y, 0);
        Debug.Log(String.Format("pos` {0}, {1}", pos.x, pos.y));
        
        SpriteRenderer spr = head.GetComponent<SpriteRenderer>();
        Vector3 size = (spr.bounds.max - spr.bounds.min);
        Debug.Log(String.Format("size {0}, {1}", size.x, size.y));

        GameObject o = Instantiate(circle, pos, Quaternion.identity);
        o.layer = 0; //ставлю дефолтное значение, делаю видимым
        // o.SetActive(false);

        // nodes.Add(o);
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

        Vector3 ds3 = new Vector3(ds.x, ds.y, 0);
        head.transform.position += ds3;

        ds *= 0.8f;

        // GameObject prev = head;
        Vector3 prev = head.transform.position;
        Vector3 t;
        /*
        Смещаю текущий элемент на позицию предыдущего. Двигаюсь от следующего за головой(первый в списке) к хвосту.
        */
        foreach(GameObject o in nodes)
        {
            t = o.transform.position;
            o.transform.position = prev;
            prev = t;
        }
    }
}
