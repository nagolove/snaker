using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBuilder : MonoBehaviour
{
    void setupUserSnake()
    {
        GameObject o = GameObject.Find("userSnake");
        if (!o) {
            Debug.LogAssertionFormat("user snake not found.");
        }
        o.AddComponent<SnakeKeyboardMovement>();
        Snake snake = o.GetComponent<Snake>();
        snake.userControlled = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        setupUserSnake();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
