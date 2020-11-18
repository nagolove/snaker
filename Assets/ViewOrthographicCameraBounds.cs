using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Common;

public class ViewOrthographicCameraBounds : MonoBehaviour
{
    LineDrawer lineDrawer = new LineDrawer();
    Camera cam;
    void Start()
    {
        cam = Camera.main;
    }
    void OnDrawGizmos()
    {
        float verticalHeightSeen = Camera.main.orthographicSize * 2.0f;
        float verticalWidthSeen = verticalHeightSeen * Camera.main.aspect;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(verticalWidthSeen, verticalHeightSeen, 0));
    }
    void OnRenderObject()
    {
        /*
        Vector2 sz = new Vector2(cam.orthographicSize * ((float)Screen.width / (float)Screen.height), cam.orthographicSize);
        sz *= 0.99f;
        Vector3 pos = transform.position;
        lineDrawer.PushLine(pos - new Vector3(-sz.x, -sz.y, 0), pos - new Vector3(sz.x, -sz.y, 0), Color.green); // top
        lineDrawer.PushLine(pos - new Vector3(-sz.x, sz.y, 0), pos - new Vector3(sz.x, sz.y, 0), Color.green); // bottom

        lineDrawer.PushLine(pos - new Vector3(-sz.x, -sz.y, 0), pos - new Vector3(-sz.x, sz.y, 0), Color.green); // left
        lineDrawer.PushLine(pos - new Vector3(sz.x, -sz.y, 0), pos - new Vector3(sz.x, sz.y, 0), Color.green); // right
        lineDrawer.DrawList();
        */
    }
}
