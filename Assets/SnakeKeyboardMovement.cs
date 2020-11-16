using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeKeyboardMovement : MonoBehaviour
{
    Snake snake;
    // Start is called before the first frame update
    void Start()
    {
        snake = GetComponent<Snake>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("left")) snake.rotateLeft();
        if (Input.GetKey("right")) snake.rotateRight();
        if (!Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown("a"))
        {
            snake.grow();
        }
        if (Input.GetKey("a")) snake.accelerate();
    }
}
