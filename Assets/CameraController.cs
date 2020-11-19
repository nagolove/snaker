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
    LineDrawer lineDrawer = new LineDrawer();
    public Vector3 leftTop, rightBottom, sz; // sz - размеры камеры в юнитах

    void Start()
    {
        mainCamera = Camera.main;
        updateBoundsRect();
    }

    void updateBoundsRect()
    {
        Vector3 sz = new Vector3(mainCamera.orthographicSize * ((float)Screen.width / (float)Screen.height),
            mainCamera.orthographicSize, 0);
        sz *= 0.9f;
        leftTop = transform.position - new Vector3(sz.x, -sz.y, 0);
        rightBottom = transform.position - new Vector3(-sz.x, sz.y, 0);
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

    public void checkPointMovement(Vector3 point)
    {
        if (point.x > rightBottom.x) {
            transform.Translate(new Vector3(point.x - rightBottom.x, 0, 0));
            updateBoundsRect();
        }
        if (point.x < leftTop.x) {
            transform.Translate(new Vector3(point.x - leftTop.x, 0, 0));
            updateBoundsRect();
        }
        if (point.y < rightBottom.y) {
            transform.Translate(new Vector3(0, point.y - rightBottom.y, 0));
            updateBoundsRect();
        }
        if (point.y > leftTop.y) {
            transform.Translate(new Vector3(0, point.y - leftTop.y, 0));
            updateBoundsRect();
        }
    }
    void OnRenderObject()
    {
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
