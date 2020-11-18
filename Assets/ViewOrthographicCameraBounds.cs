using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Common;

public class ViewOrthographicCameraBounds : MonoBehaviour
{
    LineDrawer lineDrawer = new LineDrawer();
    Camera cam;
    void Start() {
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

        Vector2 worldToPixelAmount;
        worldToPixelAmount.x = Screen.width / worldUnitsInCamera.x;
        worldToPixelAmount.y = Screen.height / worldUnitsInCamera.y;

        lineDrawer.PushLine(new Vector3(0, 0, 0), new Vector3(worldToPixelAmount.x * 1, worldToPixelAmount.y * 1, 0), Color.red);

        lineDrawer.DrawList();
    }
}
