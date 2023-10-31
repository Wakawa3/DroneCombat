using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionScript : MonoBehaviour
{
    Vector3 collision_before_velocity;
    Rigidbody rb;
    IEnumerator collision_ie = null;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        collision_before_velocity = rb.velocity;
    }
    // OnCollisionEnterはFixedUpdateよりも後に処理
    void OnCollisionEnter(Collision collision) //ここで速度を参照すると衝突後のものになる
    {
        if (collision.gameObject.tag == "Terrain" && collision_ie == null)
        { 
            collision_ie = CollisionCoroutine();
            StartCoroutine(collision_ie);
        }
    }

    IEnumerator CollisionCoroutine()
    {
        float velocity_change;
        velocity_change = Vector3.Distance(collision_before_velocity, rb.velocity);
        if (velocity_change >= 35)
        {
            if ((velocity_change - 35) / 2 <= 25)
            {
                GetComponent<HPScript>().HP -= (velocity_change - 35) / 2;
            }
            else
            {
                GetComponent<HPScript>().HP -= 25;
            }
        }
        Debug.Log(velocity_change);

        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            yield return new WaitForFixedUpdate();
        }

        collision_ie = null;
    }
}