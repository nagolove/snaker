﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Common : MonoBehaviour
{
    public struct Polar
    {
        public float angle;
        public float length;
        public Polar(float a, float len)
        {
            angle = a;
            length = len;
        }
    }

    public static Texture2D lineTex;
    public static Polar toPolar(Vector2 v)
    {
        Polar p;
        p.angle = Mathf.Atan2(v.y, v.x);
        p.length = v.magnitude;
        return p;
    }
    public static Vector2 fromPolar(Polar p)
    {
        return new Vector2(p.length * Mathf.Cos(p.angle), p.length * Mathf.Sin(p.angle));
    }

    public static Vector2 fromPolar(float angle, float length)
    {
        return fromPolar(new Polar(angle, length));
    }
    public GameObject searchObjectFromRoot(string name)
    {
        foreach (GameObject obj in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (obj.name.Equals(name))
                return obj;
        }
        return null;
    }

    public static void DrawLineGL(Vector2 from, Vector2 to, Color color)
    {
        GL.PushMatrix();
        // GL.MultMatrix(transform.localToWorldMatrix);
        GL.Begin(GL.LINES);
        {
            GL.Color(color);
            GL.Vertex3(from.x, from.y, 0);
            GL.Vertex3(to.x, to.y, 0);
        }
        GL.End();
        GL.PopMatrix();
    }
    public static float getSpriteSize(GameObject obj)
    {
        SpriteRenderer spr = obj.GetComponent<SpriteRenderer>();
        return (spr.bounds.max - spr.bounds.min).x;
    }

}
