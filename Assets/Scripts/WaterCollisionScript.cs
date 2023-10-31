using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollisionScript : MonoBehaviour
{
    Rigidbody rb;
    HPScript HPs;
    float t = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        HPs = GetComponent<HPScript>();
    }

    void Update()
    {
        if (transform.position.y < 0 && t == 0)
        {
            t += Time.deltaTime;
            HPs.HP -= 25;
            rb.velocity = new Vector3(rb.velocity.x, 100, rb.velocity.z);
        }
        else if (t > 0)
        {
            t += Time.deltaTime;

            if (t > 0.3f)
            {
                t = 0;
            }
        }
    }
}
