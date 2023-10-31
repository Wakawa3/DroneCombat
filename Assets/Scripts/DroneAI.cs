using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAI : MonoBehaviour
{
    //bool acting = false;
    IEnumerator pattern;

    Rigidbody rb;

    GameObject target;
    GameObject act_target = null;

    Vector3 default_pos;
    Vector3 default_pos2;

    Vector3 diff;
    Vector3 diff2;

    Vector3 forward2;

    float angle_from_horizontal_plane = 0;

    EnemySearchTargetScript ests;

    [SerializeField] float yaw_adjustment_speed = 2;
    [SerializeField] float pitch_adjustment_speed = 0.25f;


    [HideInInspector] public float moving_power = 1f; //DroneParticleScriptで使用
    [SerializeField] float max_moving_power = 1.5f;

    float maintaining_altitude_max_inclined_angle_rad;
    float maintaining_altitude_max_inclined_angle;

    Vector3 force;

    [SerializeField] float lowest_altitude = 700;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ests = GetComponent<EnemySearchTargetScript>();

        default_pos = transform.position;
        default_pos2 = new Vector3(default_pos.x, 0, default_pos.z);

        maintaining_altitude_max_inclined_angle_rad = Mathf.Acos(1 / max_moving_power);
        maintaining_altitude_max_inclined_angle = Mathf.Rad2Deg * maintaining_altitude_max_inclined_angle_rad;
    }

    void FixedUpdate()
    {
        force = transform.up * moving_power * (-Physics.gravity.y);

        target = ests.target;

        forward2 = new Vector3(transform.forward.x, 0, transform.forward.z);

        angle_from_horizontal_plane = Vector3.Angle(forward2, transform.forward);

        if (act_target != null)
        {
            diff = act_target.transform.position - transform.position;
            diff2 = new Vector3(diff.x, 0, diff.z);
        }

        if (pattern == null)
        {
            if (target != null)
            {
                if (Vector3.Distance(target.transform.position, transform.position) <= 3000)
                {
                    act_target = target;
                    ests.external_target_change = true;

                    if (transform.position.y < lowest_altitude + 10)
                    {
                        pattern = PatternC();
                        StartCoroutine(pattern);
                        
                        Debug.Log("start c");
                    }
                    else if (target.transform.position.y > lowest_altitude)
                    {
                        pattern = PatternA();
                        StartCoroutine(pattern);

                        Debug.Log("start a");
                    }
                    else
                    {
                        pattern = PatternB();
                        StartCoroutine(pattern);

                        Debug.Log("start b");
                    }
                }
            }
            else if (Vector3.Distance(transform.position, default_pos) > 1000)//target == null
            {
                pattern = PatternD();
                StartCoroutine(pattern);

                Debug.Log("start d");
            }
        }

        
        rb.AddForce(force);
    }

    IEnumerator PatternA() //通常
    {
        diff = act_target.transform.position - transform.position;
        diff2 = new Vector3(diff.x, 0, diff.z);

        while (Vector3.Angle(diff2, forward2) >= 5)
        {
            Vector3 normalized_diff2 = Vector3.Normalize(diff2);
            Vector3 direction = new Vector3(normalized_diff2.x, -Mathf.Tan(maintaining_altitude_max_inclined_angle_rad), normalized_diff2.z);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), yaw_adjustment_speed);

            moving_power = 1 / Mathf.Cos(angle_from_horizontal_plane * Mathf.Deg2Rad);
            yield return new WaitForFixedUpdate();
        }
        
        while (angle_from_horizontal_plane < maintaining_altitude_max_inclined_angle)
        {
            transform.Rotate(Vector3.right, pitch_adjustment_speed);

            moving_power = 1 / Mathf.Cos(angle_from_horizontal_plane * Mathf.Deg2Rad);//y方向の力がゼロになるような力
            yield return new WaitForFixedUpdate();
        }

        while (Vector3.Magnitude(diff) > 1200)
        {
            if (Vector3.Angle(diff2, forward2) > 3)
            {
                Vector3 cross = Vector3.Cross(forward2, diff2);

                if (cross.y >= 0)//targetが右にいるとき
                {
                    transform.Rotate(Vector3.up, yaw_adjustment_speed, Space.World);
                }
                else//targetが左にいるとき
                {
                    transform.Rotate(Vector3.down, yaw_adjustment_speed, Space.World);
                }
            }

            if (angle_from_horizontal_plane > maintaining_altitude_max_inclined_angle / 2 && act_target.transform.position.y > transform.position.y + 50)
            {
                transform.Rotate(Vector3.left, pitch_adjustment_speed);
            }
            else if (angle_from_horizontal_plane < maintaining_altitude_max_inclined_angle * 5 / 4 && act_target.transform.position.y < transform.position.y - 50)
            {
                transform.Rotate(Vector3.right, pitch_adjustment_speed);
            }

            if (transform.position.y <= lowest_altitude)
            {
                ChangePattern(PatternB());
                Debug.Log("change a to b");
            }

            yield return new WaitForFixedUpdate();
        }

        float t = 0;
        while (t <= 9)
        { 
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(diff), yaw_adjustment_speed);
            moving_power = 1 / Mathf.Cos(angle_from_horizontal_plane * Mathf.Deg2Rad);

            t += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        while (angle_from_horizontal_plane >= 0.01f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(diff2), yaw_adjustment_speed);
            moving_power = 1 / Mathf.Cos(angle_from_horizontal_plane * Mathf.Deg2Rad);
            yield return new WaitForFixedUpdate();
        }

        moving_power = 1;
        pattern = null;
        ests.external_target_change = false;

        Debug.Log("finish a");
    }

    IEnumerator PatternB() //targetの高さが低いとき
    {
        diff = act_target.transform.position - transform.position;
        diff2 = new Vector3(diff.x, 0, diff.z);

        if (transform.position.y < lowest_altitude)
        {
            while (transform.position.y >= lowest_altitude)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(diff2), yaw_adjustment_speed);
                moving_power = max_moving_power;

                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            while (transform.position.y >= lowest_altitude + 100)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(diff2), yaw_adjustment_speed);
                moving_power = 0.5f;

                yield return new WaitForFixedUpdate();
            }
        }

        moving_power = 1;

        while (angle_from_horizontal_plane < maintaining_altitude_max_inclined_angle)
        {
            transform.Rotate(Vector3.right, pitch_adjustment_speed);

            moving_power = 1 / Mathf.Cos(angle_from_horizontal_plane * Mathf.Deg2Rad);//y方向の力がゼロになるような力
            yield return new WaitForFixedUpdate();
        }

        if (Vector3.Magnitude(diff2) > 1100)
        {
            while (Vector3.Magnitude(diff2) > 1100)
            {
                if (Vector3.Angle(diff2, forward2) > 3)
                {
                    Vector3 cross = Vector3.Cross(forward2, diff2);

                    if (cross.y >= 0)//targetが右にいるとき
                    {
                        transform.Rotate(Vector3.up, yaw_adjustment_speed, Space.World);
                    }
                    else//targetが左にいるとき
                    {
                        transform.Rotate(Vector3.down, yaw_adjustment_speed, Space.World);
                    }
                }

                yield return new WaitForFixedUpdate();
            }       
        }
        else
        {
            while (Vector3.Magnitude(diff2) < 500)
            {
                if (Vector3.Angle(diff2, forward2) < 87)
                {
                    Vector3 cross = Vector3.Cross(forward2, diff2);

                    //距離1100以上の時と逆
                    if (cross.y >= 0)//targetが右にいるとき
                    {
                        transform.Rotate(Vector3.down, yaw_adjustment_speed, Space.World);
                    }
                    else//targetが左にいるとき
                    {
                        transform.Rotate(Vector3.up, yaw_adjustment_speed, Space.World);
                    }
                }

                yield return new WaitForFixedUpdate();
            }
        }

        while (Mathf.Cos(maintaining_altitude_max_inclined_angle_rad) < Vector3.Magnitude(diff2) / Vector3.Magnitude(diff) && act_target.transform.position.y < lowest_altitude + 120)//diff2とdiffの角度がmaintaining_altitude_max_inclined_angleを超えない間
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(diff), yaw_adjustment_speed);
            moving_power = 1 / Mathf.Cos(angle_from_horizontal_plane * Mathf.Deg2Rad);

            yield return new WaitForFixedUpdate();
        }

        while (angle_from_horizontal_plane >= 0.01f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(diff2), yaw_adjustment_speed);
            moving_power = 1 / Mathf.Cos(angle_from_horizontal_plane * Mathf.Deg2Rad);
            yield return new WaitForFixedUpdate();
        }

        moving_power = 1;
        pattern = null;
        ests.external_target_change = false;

        Debug.Log("finish b");
    }

    IEnumerator PatternC()
    {
        diff = act_target.transform.position - transform.position;
        diff2 = new Vector3(diff.x, 0, diff.z);

        moving_power = max_moving_power;
        while (transform.position.y < lowest_altitude + 10)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(diff2), yaw_adjustment_speed);
            yield return new WaitForFixedUpdate(); 
        }
        moving_power = 1;
        pattern = null;
        ests.external_target_change = false;

        Debug.Log("finish c");
    }

    IEnumerator PatternD()
    {
        Vector3 destination_diff = default_pos - transform.position;
        Vector3 destination_diff2 = new Vector3(destination_diff.x, 0, destination_diff.z);

        while (transform.position.y < lowest_altitude + 10)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(destination_diff2), yaw_adjustment_speed);
            moving_power = max_moving_power;
            yield return new WaitForFixedUpdate();
        }

        while (Vector3.Angle(destination_diff2, forward2) >= 5)
        {
            destination_diff = default_pos - transform.position;
            destination_diff2 = new Vector3(destination_diff.x, 0, destination_diff.z);


            Vector3 normalized_diff2 = Vector3.Normalize(destination_diff2);
            Vector3 direction = new Vector3(normalized_diff2.x, -Mathf.Tan(maintaining_altitude_max_inclined_angle_rad), normalized_diff2.z);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), yaw_adjustment_speed);

            moving_power = 1 / Mathf.Cos(angle_from_horizontal_plane * Mathf.Deg2Rad);

            if (target != null)
            {
                if (target.transform.position.y > lowest_altitude)
                {
                    ChangePattern(PatternA());
                    Debug.Log("change d to a");
                }
                else
                {
                    ChangePattern(PatternB());
                    Debug.Log("change d to b");
                }
            }

            yield return new WaitForFixedUpdate();
        }

        while (angle_from_horizontal_plane < maintaining_altitude_max_inclined_angle)
        {
            transform.Rotate(Vector3.right, pitch_adjustment_speed);

            moving_power = 1 / Mathf.Cos(angle_from_horizontal_plane * Mathf.Deg2Rad);//y方向の力がゼロになるような力
            yield return new WaitForFixedUpdate();
        }

        while (Vector3.Magnitude(destination_diff2) > 1000)
        {
            destination_diff = default_pos - transform.position;
            destination_diff2 = new Vector3(destination_diff.x, 0, destination_diff.z);

            if (Vector3.Angle(destination_diff2, forward2) > 0.1f)
            {
                Vector3 cross = Vector3.Cross(forward2, destination_diff2);

                if (cross.y >= 0)//targetが右にいるとき
                {
                    transform.Rotate(Vector3.up, yaw_adjustment_speed, Space.World);
                }
                else//targetが左にいるとき
                {
                    transform.Rotate(Vector3.down, yaw_adjustment_speed, Space.World);
                }
            }

            if (target != null)
            {
                if (target.transform.position.y > lowest_altitude)
                {
                    ChangePattern(PatternA());
                    Debug.Log("change d to a");
                }
                else
                {
                    ChangePattern(PatternB());
                    Debug.Log("change d to b");
                }
            }

            yield return new WaitForFixedUpdate();
        }

        while (angle_from_horizontal_plane >= 0.01f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(destination_diff2), yaw_adjustment_speed);
            moving_power = 1 / Mathf.Cos(angle_from_horizontal_plane * Mathf.Deg2Rad);
            yield return new WaitForFixedUpdate();
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(destination_diff2), yaw_adjustment_speed);

        moving_power = 1;
        pattern = null;

        Debug.Log("finish d");
    }

    void ChangePattern(IEnumerator next_pattern)
    {
        StopCoroutine(pattern);
        pattern = null;
        pattern = next_pattern;
        StartCoroutine(pattern);   
    }
}