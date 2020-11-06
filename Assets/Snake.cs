using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static EasingFunction;

public class Snake : MonoBehaviour
{
    GameObject head;
    Vector2 ds;
    public float speed = 10;
    List<GameObject> nodes = new List<GameObject>();
    Vector3 last;
    void Start()
    {
        head = GameObject.Find("head");
    }

    void AddNode()
    {
        GameObject circle = GameObject.Find("head");
        // Debug.Log(String.Format("circle {}", circle));
        Vector3 pos = transform.position;
        // pos.x -= ds.x;
        // pos.y -= ds.y;
        Vector3 dir = last - transform.position;
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
