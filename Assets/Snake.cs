using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static EasingFunction;
using UnityEngine.UI;
using TMPro;

public class Snake : MonoBehaviour
{
    GameObject head;
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
    void Start()
    {
        head = GameObject.Find("head");

        putTextAtPoint(new Vector2(transform.position.x, transform.position.y));

        Debug.Log("текст выведен");
    }

    void AddNode()
    {
        /*
        Сперва найти безопасную область вокруг последнего элемента. 
        Как найти такую область? Обработка столкновений и пересечений.
        Потом поместить туда новый объект.
        */

        Vector3 pos = transform.position;
        Vector3 dir = last - transform.position;
        pos = dir.normalized * 2;
        GameObject o = Instantiate(head, pos, Quaternion.identity);
        o.GetComponent<Snake>().enabled = false; // не лучший вариант, создается много лишнего через такой объект.
        nodes.Add(o);
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

        GameObject prev = head;
        /*
        Смещаю следующие элементы на позицию предыдущего. Двигаюсь от головы к хвосту.
        */
        foreach(GameObject o in nodes)
        {
            o.transform.position = head.transform.position;
            prev = o;
        }
    }
}
