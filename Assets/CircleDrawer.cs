using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDrawer : MonoBehaviour
{
    LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = new LineRenderer();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
    }

    public void Circle(Vector3 pos, float r, Color color)
    {
        int i = 0;
        for(float angle = 0.0f; angle < Mathf.PI * 2.0f; angle += r * 3.0f)
        { 
            Vector3 vertex = new Vector3(Mathf.Cos(angle) * r, Mathf.Sin(angle) * r);
            lineRenderer.SetPosition(i++, pos + vertex);
        }
    }
}
