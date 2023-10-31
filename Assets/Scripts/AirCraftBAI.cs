using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//EnemySearchTargetScriptより後に処理
public class AirCraftBAI : MonoBehaviour
{
    [SerializeField] float default_speed = 8;
    float speed;
    [SerializeField] float max_rotation_angle = 0.1f;

    Vector3 diff;

    bool acting = false;

    EnemySearchTargetScript ests;
    GameObject target;
    GameObject act_target = null;

    void Start()
    {
        speed = default_speed;
        ests = GetComponent<EnemySearchTargetScript>();
    }

    void FixedUpdate()
    {
        target = ests.target;

        if (act_target != null)
        {
            diff = act_target.transform.position - transform.position;
        }

        if (target != null)
        {
            diff = target.transform.position - transform.position;

            if (acting == false)
            {
                if (Vector3.Magnitude(diff) >= 800)
                {
                    speed = default_speed * 3 / 4;
                    transform.Rotate(Vector3.left, max_rotation_angle);
                }
                else
                {
                    speed = default_speed;
                }
            }
            
        }


    
        transform.position = transform.position + transform.forward * speed;
    }

    void Update()
    {
        if (acting)
        {
            ests.external_target_change = true;

            if (act_target != null)//UpdateとFixedUpdate間の調整
            {
                ests.target = act_target;
            }
            
        }
        
    }

    
    IEnumerator PatternA()//ピッチアップ
    {
        acting = true;
        act_target = target;

        float angle_a = 180;//適当な数字

        for (;angle_a <= max_rotation_angle * 50 ; )
        {
            angle_a = Vector3.Angle(transform.forward, Vector3.ProjectOnPlane(diff, transform.right));

            transform.Rotate(Vector3.left, max_rotation_angle);
            yield return new WaitForFixedUpdate();
        }

        for (int i = 0; i < 20; i++)
        {
            transform.Rotate(Vector3.left, max_rotation_angle);
            yield return new WaitForFixedUpdate();
        }

        speed = default_speed;

       /* float angle_b = 180;

        for (;angle_b <= max_rotation_angle * 30; )
        {
            angle_b = Vector3.Angle(transform.up, Vector3.ProjectOnPlane(diff, transform.forward));

            transform.Rotate(Vector3.forward, max_rotation_angle);
            yield return new WaitForFixedUpdate();
        }*/

        for (;Vector3.Magnitude(diff) <= 100 || Vector3.Magnitude(diff) >= 2000;)
        {
            yield return new WaitForFixedUpdate();
        }
        acting = false;
        act_target = null;

    }
}
