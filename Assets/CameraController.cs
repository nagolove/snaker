using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera mainCamera;
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
    }
}
