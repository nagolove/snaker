using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera mainCamera;
    public float speed = 10.0f;
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("z"))
            mainCamera.orthographicSize *= 0.9f;
        if (Input.GetKey("x"))
            mainCamera.orthographicSize *= 1.1f;

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey("a"))
            transform.Translate(new Vector3(speed * Time.deltaTime, 0));
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey("d"))
            transform.Translate(new Vector3(-speed * Time.deltaTime, 0));
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey("w"))
            transform.Translate(new Vector3(0, speed * Time.deltaTime));
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey("s"))
            transform.Translate(new Vector3(0, -speed * Time.deltaTime));
    }
}
