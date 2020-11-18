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
    void Start()
    {
        mainCamera = Camera.main;
        updateBoundsRect();
    }

    void updateBoundsRect()
    {
        Vector3 sz = new Vector3(mainCamera.orthographicSize * ((float)Screen.width / (float)Screen.height),
            mainCamera.orthographicSize, 0);
        // Vector2 pos = transform.position;
        Vector3 pos = transform.position;
        Vector3 leftUp = pos - new Vector3(-sz.x, sz.y, 0);
        Vector3 rightDown = pos - new Vector3(sz.x, -sz.y, 0);
        bound.Set(leftUp.x, leftUp.y, Math.Abs(leftUp.x - rightDown.x), Math.Abs(leftUp.y - rightDown.y));
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
        // lineDrawer.PushLine(pos - new Vector3(-sz.x, sz.y, 0), pos - new Vector3(sz.x, sz.y, 0), Color.green); // bottom

        // lineDrawer.PushLine(pos - new Vector3(-sz.x, -sz.y, 0), pos - new Vector3(-sz.x, sz.y, 0), Color.green); // left
        // lineDrawer.PushLine(pos - new Vector3(sz.x, -sz.y, 0), pos - new Vector3(sz.x, sz.y, 0), Color.green); // right
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
