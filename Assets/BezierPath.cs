using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierPath : MonoBehaviour
{
    public GameObject[] path;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        Vector3 prev = path[0].transform.position;
        Gizmos.color = Color.red;
        for(int i = 0; i < path.Length; ++i)
        {
            Gizmos.DrawLine(prev, path[i].transform.position);
            prev = path[i].transform.position;
        }
        Gizmos.DrawLine(path[0].transform.position, path[path.Length - 1].transform.position);
    }
}
