using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirCraftAAI : MonoBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] float rotation_angle = 0.1f;
    [SerializeField] MovingDirection moving_dorection = MovingDirection.right;

    enum MovingDirection
    {
        straight,
        right,
        left
    }

    void Start()
    {
        
    }


    void FixedUpdate()
    {
        transform.position = transform.position + transform.forward * speed;

        if (moving_dorection == MovingDirection.right)
        {
            transform.Rotate(Vector3.up, rotation_angle);
        }
        else if (moving_dorection == MovingDirection.left)
        {
            transform.Rotate(-Vector3.up, rotation_angle);
        }

    }
}
