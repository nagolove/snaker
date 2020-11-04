using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    GameObject head;
    Vector2 ds;
    public float speed = 10;
    void Start()
    {
        head = GameObject.Find("head");
    }

    void Update()
    {
        if (Input.GetKey("left"))
            ds = new Vector2(-speed * Time.deltaTime, 0);
        if (Input.GetKey("right"))
            ds = new Vector2(speed * Time.deltaTime, 0);
        if (Input.GetKey("up"))
            ds = new Vector2(0, speed * Time.deltaTime);
        if (Input.GetKey("down"))
            ds = new Vector2(0, -speed * Time.deltaTime);

        head.transform.position += new Vector3(ds.x, ds.y, 0);
    }
}
