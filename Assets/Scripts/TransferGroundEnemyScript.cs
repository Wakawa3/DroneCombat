using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferGroundEnemyScript : MonoBehaviour
{
    [SerializeField] Vector3 destination_pos = Vector3.zero;
    Vector3 default_pos;
    float distance;

    [SerializeField] float transfer_speed = 0.3f;
    //[SerializeField] float wait_time = 0;

    float frame = 0;
    bool going = true;

    void Start()
    {
        default_pos = transform.position;

        distance = Vector3.Distance(default_pos, destination_pos);
    }

    void FixedUpdate()
    {
        if (going)
        {
            frame++;

            float t = frame * transfer_speed / distance;

            transform.position = Vector3.Lerp(default_pos, destination_pos, t);
            
            if (t >= 1)
            {
                going = false;
            }

        }
        else
        {
            frame--;

            float t = frame * transfer_speed / distance;

            transform.position = Vector3.Lerp(default_pos, destination_pos, t);

            if (t <= 0)
            {
                going = true;
            }
        }

    }
}
