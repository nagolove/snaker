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
            GameObject.Instantiate(dish, position, Quaternion.identity);
            time = newTime;
        }
    }
}
