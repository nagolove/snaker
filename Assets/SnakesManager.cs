using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakesManager : MonoBehaviour
{
    public int maxNum;
    public GameObject snakePrefab;
    public List<GameObject> snakes;
    public Vector3 secondCorner;
    static int num = 0;
    void Awake()
    {

    }
    void Start()
    {
        for (int i = 0; i < maxNum; ++i)
        {
            spawn();
        }
    }

    string generateName()
    {
        num++;
        return "snake" + num.ToString();
    }
    Vector3 getRandomPositon()
    {
        return new Vector3(UnityEngine.Random.Range(transform.position.x, transform.position.x + secondCorner.x),
            UnityEngine.Random.Range(transform.position.y, transform.position.y + secondCorner.y));
    }
    void spawn()
    {
        Vector3 pos = getRandomPositon();
        GameObject o = Instantiate(snakePrefab, pos, Quaternion.identity);
        o.name = generateName();
        Snake snake = o.GetComponent<Snake>();

        if (!snake) {
            Debug.LogWarningFormat("no Snake in prefab");
        }

        snake.setRotation(UnityEngine.Random.Range(0, 360));

        for (int i = UnityEngine.Random.Range(0, 100); i > 0; --i)
        {
            // snake.grow();
        }
        // snake.grow();
        snakes.Add(o);
    }

    void growRandomSnake()
    {
        GameObject o = snakes[UnityEngine.Random.Range(0, snakes.Count)];
        Snake sn = o.GetComponent<Snake>();
        sn.grow();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p"))
            growRandomSnake();

        if (Input.GetKeyDown("l"))
        {
            for (int i = UnityEngine.Random.Range(0, snakes.Count); i > 0; --i)
            {
                Snake snake = snakes[i].GetComponent<Snake>();        
                snake.grow();
            }
        }
    }

    public bool inArea(Vector3 point)
    {
        Vector3 corner1 = transform.position;
        Vector3 corner2 = transform.position + secondCorner;
        return (corner1.x < point.x && corner1.y < point.y && corner2.x > point.x && corner2.y > point.y);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 t1 = secondCorner, t2 = secondCorner;

        t1.y = 0;
        Gizmos.DrawLine(transform.position, transform.position + t1);

        t2.x = 0;
        t2.y = secondCorner.y;
        t1 = secondCorner;
        Gizmos.DrawLine(transform.position + t2, transform.position + t1);

        t1 = secondCorner;
        t1.x = 0;
        Gizmos.DrawLine(transform.position, transform.position + t1);

        t2 = secondCorner;
        t2.y = 0;
        t1 = secondCorner;
        Gizmos.DrawLine(transform.position + t2, transform.position + t1);

    }
}
