using System.Collections;
using System.Collections.Generic;
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
            Debug.Log(string.Format("orthographic size {0}", mainCamera.orthographicSize));
        }
        if (Input.GetKey("x") && (mainCamera.orthographicSize * (1.0f + scaleConst) < maxZoom))
        {
            mainCamera.orthographicSize *= 1.0f + scaleConst;
            Debug.Log(string.Format("orthographic size {0}", mainCamera.orthographicSize));
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
}
