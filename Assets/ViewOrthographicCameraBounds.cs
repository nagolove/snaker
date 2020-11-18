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
        Vector2 worldUnitsInCamera;
        worldUnitsInCamera.y = cam.orthographicSize * 2;
        worldUnitsInCamera.x = worldUnitsInCamera.y * Screen.width / Screen.height;

        Vector2 w2pix;
        w2pix.x = Screen.width / (1.0f / worldUnitsInCamera.x);
        w2pix.y = Screen.height / (1.0f / worldUnitsInCamera.y);

        LogOnce("worldToPixelAmount {0}, {1}", w2pix.x, w2pix.y);
        Vector2 sz = new Vector2(cam.orthographicSize, cam.orthographicSize * ((float)Screen.width / (float)Screen.height));
        Debug.Log(string.Format("sz {0},{1}", sz.x, sz.y));
        
        // float size = cam.orthographicSize * 0.9f;
        sz *= 0.9f;

        lineDrawer.PushLine(new Vector3(-sz.x, -sz.y, 0), new Vector3(sz.x, -sz.y, 0), Color.green); // top
        lineDrawer.PushLine(new Vector3(-sz.x, sz.y, 0), new Vector3(sz.x, sz.y, 0), Color.green); // bottom

        lineDrawer.PushLine(new Vector3(0,0, 0), new Vector3(sz.x, -sz.y, 0), Color.green); // top
        lineDrawer.PushLine(new Vector3(0, 0, 0), new Vector3(sz.x, sz.y, 0), Color.green); // bottom

        lineDrawer.PushLine(new Vector3(-sz.x, -sz.y, 0), new Vector3(-sz.x, sz.y, 0), Color.green); // left
        lineDrawer.PushLine(new Vector3(sz.x, -sz.y, 0), new Vector3(sz.x, sz.y, 0), Color.green); // right


        lineDrawer.DrawList();
    }
}
