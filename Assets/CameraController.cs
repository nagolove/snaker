using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using static Common;

public class CameraController : MonoBehaviour
{
    Camera mainCamera;
    public float speed = 10.0f;
    public float minZoom, maxZoom;
    public float scaleConst = 0.1f;
    public Rect bound, headBound; // границы камеры и уменьшенные границы камеры для передвижения камеры вслед за головой.
    LineDrawer lineDrawer = new LineDrawer();


    GameObject circle1, circle2;

    void Start()
    {
        mainCamera = Camera.main;

        GameObject testCircle = GameObject.Find("testCircle");
        if (!testCircle)
        {
            Debug.LogWarning("not found");
        }
        else
        {
            Debug.LogWarning("found");
        }
        circle1 = Instantiate(testCircle);
        circle1.layer = 0;
        circle1.GetComponent<SpriteRenderer>().color = Color.yellow;

        circle2 = Instantiate(testCircle);
        circle2.layer = 0;
        circle2.GetComponent<SpriteRenderer>().color = Color.red;

        updateBoundsRect();
    }

    void updateBoundsRect()
    {
        Vector3 sz = new Vector3(mainCamera.orthographicSize * ((float)Screen.width / (float)Screen.height),
            mainCamera.orthographicSize, 0);
        // Vector2 pos = transform.position;
        Vector3 pos = transform.position;
        Vector3 leftUp = pos - new Vector3(sz.x, -sz.y, 0);
        Vector3 rightDown = pos - new Vector3(-sz.x, sz.y, 0);
        bound.Set(leftUp.x, leftUp.y, Math.Abs(leftUp.x - rightDown.x), Math.Abs(leftUp.y - rightDown.y));

        circle1.transform.position = leftUp;
        circle2.transform.position = rightDown;
    }

    void Update()
    {
        if (Input.GetKey("z") && (mainCamera.orthographicSize * (1.0f - scaleConst) > minZoom))
        {
            mainCamera.orthographicSize *= 1.0f - scaleConst;
            // Debug.Log(string.Format("orthographic size {0}", mainCamera.orthographicSize));
            updateBoundsRect();
        }
        if (Input.GetKey("x") && (mainCamera.orthographicSize * (1.0f + scaleConst) < maxZoom))
        {
            mainCamera.orthographicSize *= 1.0f + scaleConst;
            // Debug.Log(string.Format("orthographic size {0}", mainCamera.orthographicSize));
            updateBoundsRect();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKey("a"))
                transform.Translate(new Vector3(speed * Time.deltaTime, 0));
            if (Input.GetKey("d"))
                transform.Translate(new Vector3(-speed * Time.deltaTime, 0));
            if (Input.GetKey("w"))
                transform.Translate(new Vector3(0, speed * Time.deltaTime));
            if (Input.GetKey("s"))
                transform.Translate(new Vector3(0, -speed * Time.deltaTime));
        }
    }

    void OnRenderObject()
    {
        Vector2 min = bound.min;
        Vector2 max = bound.max;
        Debug.Log(String.Format("min {0}, {1} max {0}, {1}", min.x, min.y, max.x, max.y));
        lineDrawer.PushLine(min, max, Color.blue); // top
        lineDrawer.PushLine(max, min, Color.white); // top
        // lineDrawer.PushLine(pos - new Vector3(-sz.x, sz.y, 0), pos - new Vector3(sz.x, sz.y, 0), Color.green); // bottom

        // lineDrawer.PushLine(pos - new Vector3(-sz.x, -sz.y, 0), pos - new Vector3(-sz.x, sz.y, 0), Color.green); // left
        // lineDrawer.PushLine(pos - new Vector3(sz.x, -sz.y, 0), pos - new Vector3(sz.x, sz.y, 0), Color.green); // right

        Vector2 sz = new Vector2(mainCamera.orthographicSize * ((float)Screen.width / (float)Screen.height), mainCamera.orthographicSize);
        sz *= 0.9f;
        Vector3 pos = transform.position;
        lineDrawer.PushLine(pos - new Vector3(-sz.x, -sz.y, 0), pos - new Vector3(sz.x, -sz.y, 0), Color.green); // top
        lineDrawer.PushLine(pos - new Vector3(-sz.x, sz.y, 0), pos - new Vector3(sz.x, sz.y, 0), Color.green); // bottom
        lineDrawer.PushLine(pos - new Vector3(-sz.x, -sz.y, 0), pos - new Vector3(-sz.x, sz.y, 0), Color.green); // right
        lineDrawer.PushLine(pos - new Vector3(sz.x, -sz.y, 0), pos - new Vector3(sz.x, sz.y, 0), Color.green); // left
        lineDrawer.DrawList();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Debug.Log(String.Format("enter with tag {0}", collision.gameObject.tag));
        // Debug.Log(String.Format("enter with name {0}", collision.gameObject.name));
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        // Debug.Log(String.Format("exit with tag {0}", collision.gameObject.tag));
        // Debug.Log(String.Format("exit with name {0}", collision.gameObject.name));
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        // Debug.Log(String.Format("stay with tag {0}", collision.gameObject.tag));
        // Debug.Log(String.Format("stay with name {0}", collision.gameObject.name));
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log(String.Format("trig enter with tag {0}", collision.gameObject.tag));
        // Debug.Log(String.Format("trig enter with name {0}", collision.gameObject.name));
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        // Debug.Log(String.Format("trig exit with tag {0}", collision.gameObject.tag));
        // Debug.Log(String.Format("trig exit with name {0}", collision.gameObject.name));
    }
    void OnTriggerStay2D(Collider2D collison)
    {
        // Debug.Log(String.Format("trig stay with tag {0}", collison.gameObject.tag));
        // Debug.Log(String.Format("trig stay with name {0}", collison.gameObject.name));
        if (collison.gameObject.tag == "Snake")
        {
            // Vector3 vec = 
            // Vector2 relVel = collison.gameObject;
            // Debug.Log(string.Format("col with {0}", collison.gameObject.name));
            // transform.Translate(new Vector3(relVel.x, relVel.y));
        }
    }
}
