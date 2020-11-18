using System.Collections;
using System.Collections.Generic;
using System;
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

    public static Vector3 Bezier3PathCalculation(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) =>
        Mathf.Pow(1.0f - t, 3) * p0 + 3.0f * Mathf.Pow(1.0f - t, 2) * t * p1 +
            3.0f * (1.0f - t) * Mathf.Pow(t, 2) * p2 + Mathf.Pow(t, 3) * p3;

    public static Vector3 Bezier2PathCalculation(Vector3 p0, Vector3 p1, Vector3 p2, float t) =>
        Mathf.Pow(1 - t, 2) * p0 + 2 * t * (1 - t) * p1 + Mathf.Pow(t, 2) * p2;

    public class LineDrawer
    {
        struct Line
        {
            public Vector2 from, to;
            public Color color;
            public Line(Vector2 from, Vector2 to, Color color)
            {
                this.from = from;
                this.to = to;
                this.color = color;
            }
        }
        List<Line> lines = new List<Line>();

        public void PushLine(Vector2 from, Vector2 to, Color color)
        {
            lines.Add(new Line(from, to, color));
        }
        public void PushLine(Vector3 from, Vector3 to, Color color)
        {
            lines.Add(new Line(from, to, color));
        }
        public void PushRect(Rect r, Color color)
        {
            lines.Add(new Line(new Vector2(r.x, r.y), new Vector2(r.x + r.width, r.y), color));
            lines.Add(new Line(new Vector2(r.x + r.width, r.y), new Vector2(r.x + r.width, r.y + r.height), color));
            lines.Add(new Line(new Vector2(r.x + r.width, r.y + r.height), new Vector2(r.x, r.y + r.height), color));
            lines.Add(new Line(new Vector2(r.x, r.y + r.height), new Vector2(r.x, r.y), color));
        }
        public void DrawList()
        {
            foreach (Line line in lines)
            {
                Common.DrawLineGL(line.from, line.to, line.color);
            }
            lines.Clear();
        }
    }

    static HashSet<string> logged = new HashSet<string>();
    public static void LogOnce(string format, params object[] args)
    {
        string formatted = String.Format(format, args);
        if (!logged.Contains(formatted))
        {
            Debug.Log(formatted);
            logged.Add(formatted);
        }
    }
}
