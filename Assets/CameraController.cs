using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera mainCamera;
    public float speed = 10.0f;
    public float minZoom = 1.0f, maxZoom = 300.0f;
    public float scaleConst = 0.1f;
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("z") && (mainCamera.orthographicSize * (1.0f - scaleConst) > minZoom))
        {
            mainCamera.orthographicSize *= 1.0f - scaleConst;
            // Debug.Log(string.Format("orthographic size {0}", mainCamera.orthographicSize));
        }
        if (Input.GetKey("x") && (mainCamera.orthographicSize * (1.0f + scaleConst) < maxZoom))
        {
            mainCamera.orthographicSize *= 1.0f + scaleConst;
            // Debug.Log(string.Format("orthographic size {0}", mainCamera.orthographicSize));
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKey("a"))
                transform.Translate(new Vector3(speed * Time.deltaTime, 0));
            if (Input.GetKey("d"))
                transform.Translate(new Vector3(-speed * Time.deltaTime, 0));
            if (Input.GetKey("w"))
                transform.Translate(new Vector3(0, -speed * Time.deltaTime));
            if (Input.GetKey("s"))
                transform.Translate(new Vector3(0, speed * Time.deltaTime));
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(String.Format("enter with tag {0}", collision.gameObject.tag));
        Debug.Log(String.Format("enter with name {0}", collision.gameObject.name));
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log(String.Format("exit with tag {0}", collision.gameObject.tag));
        Debug.Log(String.Format("exit with name {0}", collision.gameObject.name));
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log(String.Format("stay with tag {0}", collision.gameObject.tag));
        Debug.Log(String.Format("stay with name {0}", collision.gameObject.name));
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(String.Format("trig enter with tag {0}", collision.gameObject.tag));
        Debug.Log(String.Format("trig enter with name {0}", collision.gameObject.name));
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(String.Format("trig exit with tag {0}", collision.gameObject.tag));
        Debug.Log(String.Format("trig exit with name {0}", collision.gameObject.name));
    }
    void OnTriggerStay2D(Collider2D collison)
    {
        Debug.Log(String.Format("trig stay with tag {0}", collison.gameObject.tag));
        Debug.Log(String.Format("trig stay with name {0}", collison.gameObject.name));
        if (collison.gameObject.tag == "Snake")
        {
            // Vector3 vec = 
            // Vector2 relVel = collison.gameObject;
            // Debug.Log(string.Format("col with {0}", collison.gameObject.name));
            // transform.Translate(new Vector3(relVel.x, relVel.y));
        }
    }
}
