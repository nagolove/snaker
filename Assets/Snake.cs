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
        GameObject ngo = new GameObject("myTextGO");
        // ngo.transform.SetParent(this.transform); 
        ngo.transform.Translate(new Vector3(p.x, p.y, 0));
        // ngo.transform.position.x = p.x;
        // ngo.transform.position.y = p.y;
        TextMeshPro t = ngo.AddComponent<TextMeshPro>();
        t.transform.SetParent(transform);
        t.fontSize = 8;
        t.text = String.Format("{0}", num);
        // t.transform.localScale = new Vector3(0.1f, 0.1f, 0);
        // RectTransform tr = t.rectTransform;
        t.alignment = TextAlignmentOptions.Center;
        t.color = Color.black;
    }
    void Start()
    {
        head = GameObject.Find("head");
        /*
        Text t =head.AddComponent<Text>();
        t.transform.position = transform.position;
        t.color = Color.red;
        t.text = String.Format("{0}", num);
        */

        /*
        GameObject ngo = new GameObject("myTextGO");
        ngo.transform.SetParent(this.transform); 
        TextMeshPro t = ngo.AddComponent<TextMeshPro>();
        // myText.transform.SetParent(transform);
        // t.fontSize = 38;
        t.text = "Ta-dah!";
        // t.transform.localScale = new Vector3(0.1f, 0.1f, 0);
        RectTransform tr = t.rectTransform;
        t.alignment = TextAlignmentOptions.TopLeft;
        */

        putTextAtPoint(new Vector2(transform.position.x, transform.position.y));

        Debug.Log("текст выведен");
    }

    void AddNode()
    {
        GameObject circle = GameObject.Find("head");
        // Debug.Log(String.Format("circle {}", circle));
        Vector3 pos = transform.position;
        // pos.x -= ds.x;
        // pos.y -= ds.y;
        Vector3 dir = last - transform.position;
        Debug.DrawLine(transform.position, transform.position + dir, Color.red, 1000);
        pos = dir.normalized * 2;
        GameObject o = Instantiate(circle, pos, Quaternion.identity);
        o.GetComponent<Snake>().enabled = false; // не лучший вариант 
        // o.SetActive(true);
        nodes.Add(o);
    }

    IEnumerable Fade(Vector2 d)
    {
        for (int i = 0; i < 100; i++)
        {
            d *= 0.19f;
            yield return null;
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

        Vector3 ds3 = new Vector3(ds.x, ds.y, 0);
        head.transform.position += ds3;

        ds *= 0.8f;

        foreach(GameObject o in nodes)
        {
            o.transform.position += ds3;
        }
    }
}
