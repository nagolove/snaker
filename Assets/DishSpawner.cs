using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static Common;

public class DishSpawner : MonoBehaviour
{
    [Range(0, 1000)]
    public int spawnSpeed = 5; // сколько продуктов в минуту создавать
    float dishDelay;
    float time;
    GameObject dish;
    float rad;
    void Start()
    {
        time = Time.unscaledTime;
        dishDelay = 60.0f / spawnSpeed;
        dish = GameObject.Find("dish");
        rad = getSpriteSize(dish);
    }

    Vector3 generatePosInCircle(Vector3 center)
    {
        Polar polar = new Polar((float)(UnityEngine.Random.Range(0.0f, 2.0f) * Math.PI), UnityEngine.Random.Range(0.0f, rad));
        Vector2 res = Common.fromPolar(polar);
        return new Vector3(res.x, res.y);
    }
    // Update is called once per frame
    void Update()
    {
        float newTime = Time.unscaledTime;
        if (newTime - time >= dishDelay)
        {
            Vector3 position = generatePosInCircle(transform.position);
            GameObject o = GameObject.Instantiate(dish, position, Quaternion.identity);
            o.layer = 0; // включаю видимость
            time = newTime;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        int i = 0;
        Vector3 from = new Vector3(Mathf.Cos(0.0f) * rad, Mathf.Sin(0.0f) * rad);
        for (float angle = 0.0f; angle < Mathf.PI * 2.0f; angle += rad * 3.0f)
        {
            Vector3 to = new Vector3(Mathf.Cos(angle) * rad, Mathf.Sin(angle) * rad);
            // Gizmos.DrawLine(from, to);
            from = to;
        }
    }
}