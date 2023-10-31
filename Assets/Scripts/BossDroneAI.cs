using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDroneAI : MonoBehaviour, IDetectMissile
{
    //bool acting = false;
    IEnumerator pattern;

    Rigidbody rb;

    //GameObject target;
    GameObject act_target = null;

    Vector3 diff;
    Vector3 diff2;

    Vector3 forward2;

    float angle_from_horizontal_plane = 0;

    //bool missile_detected = false;

    EnemySearchTargetScript ests;
    EnemyShootingMissileScript esms;

    [SerializeField] float yaw_adjustment_speed = 2;
    [SerializeField] float pitch_adjustment_speed = 0.25f;


    [HideInInspector] public float moving_power = 1f; //DroneParticleScriptで使用
    [SerializeField] float max_moving_power = 4f;

    float maintaining_altitude_max_inclined_angle_rad;
    float maintaining_altitude_max_inclined_angle;

    Vector3 force;

    [SerializeField] float lowest_altitude = 700;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ests = GetComponent<EnemySearchTargetScript>();
        esms = GetComponent<EnemyShootingMissileScript>();

        maintaining_altitude_max_inclined_angle_rad = Mathf.Acos(1 / max_moving_power);
        maintaining_altitude_max_inclined_angle = Mathf.Rad2Deg * maintaining_altitude_max_inclined_angle_rad;

        ests.external_target_change = true;
        ests.target = GameObjectManagement.player;
        act_target = GameObjectManagement.player;
    }

    void FixedUpdate()
    {
        force = transform.up * moving_power * (-Physics.gravity.y);

        forward2 = new Vector3(transform.forward.x, 0, transform.forward.z);

        angle_from_horizontal_plane = Vector3.Angle(forward2, transform.forward);

        diff = act_target.transform.position - transform.position;
        diff2 = new Vector3(diff.x, 0, diff.z);
        
        if (pattern == null)
        {
            if (transform.position.y < lowest_altitude + 10)
            {
                pattern = Pattern103();
                StartCoroutine(pattern);

                Debug.Log("start 103");
            }
            else if (act_target.transform.position.y > lowest_altitude)
            {
                float diff_mag = Vector3.Magnitude(diff2);
                if (diff_mag > 1300)
                {
                    pattern = Pattern101();
                    StartCoroutine(pattern);

                    Debug.Log("start 101");
                }
                else
                {
                    pattern = Pattern102();
                    StartCoroutine(pattern);

                    Debug.Log("start 102");
                }
            }
 /*           else
            {
                //pattern = PatternB();
                StartCoroutine(pattern);

                Debug.Log("start b");
            }*/

        }

        rb.AddForce(force);

        //missile_detected = false;
    }

    public void DetectMissile()
    {
        //missile_detected = true;
    }

    IEnumerator Pattern101()//移動 通常
    {
        while (angle_from_horizontal_plane < maintaining_altitude_max_inclined_angle)
        {
            Vector3 normalized_diff2 = Vector3.Normalize(diff2);
            Vector3 direction = new Vector3(normalized_diff2.x, -Mathf.Tan(maintaining_altitude_max_inclined_angle_rad), normalized_diff2.z);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), yaw_adjustment_speed);

            moving_power = 1 / Mathf.Cos(angle_from_horizontal_plane * Mathf.Deg2Rad);
            yield return new WaitForFixedUpdate();
        }

        while (Vector3.Magnitude(diff2) > 1300)
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
            else if (angle_from_horizontal_plane < 85 && act_target.transform.position.y < transform.position.y - 50)
            {
                transform.Rotate(Vector3.right, pitch_adjustment_speed);
            }

            yield return new WaitForFixedUpdate();
        }

        pattern = null;
    }

    IEnumerator Pattern102()//移動 接近時
    {
        Vector3 variation = Random.onUnitSphere * 800;

        Vector3 destination_diff = diff + variation;
        Vector3 destination_diff2 = destination_diff;
        destination_diff2.y = 0;

        while (Vector3.Angle(destination_diff2, forward2) >= 5)
        {
            destination_diff = diff + variation;
            destination_diff2 = destination_diff;
            destination_diff2.y = 0;

            Vector3 normalized_diff2 = Vector3.Normalize(destination_diff2);
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

        destination_diff = diff + variation;
        destination_diff2 = destination_diff;
        destination_diff2.y = 0;

        moving_power = max_moving_power;

        while (Vector3.Magnitude(destination_diff2) > 300)
        {
            if (Vector3.Angle(destination_diff2, forward2) > 3)
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

            if (angle_from_horizontal_plane > maintaining_altitude_max_inclined_angle / 2 && destination_diff.y > 20)
            {
                transform.Rotate(Vector3.left, pitch_adjustment_speed);
            }
            else if (angle_from_horizontal_plane < 85 && destination_diff.y < -20)
            {
                transform.Rotate(Vector3.right, pitch_adjustment_speed);
            }

            destination_diff = diff + variation;
            destination_diff2 = destination_diff;
            destination_diff2.y = 0;

            yield return new WaitForFixedUpdate();
        }

        int n = Random.Range(0,2);
        
        if (n == 0)
        {
            ChangePattern(Pattern201());
            Debug.Log("change 102 to 201");
        }

        pattern = null;

    }

    IEnumerator Pattern103()//上昇
    {
        moving_power = max_moving_power;

        float default_pos_y = transform.position.y;

        while (Vector3.Angle(diff2, forward2) < 5 && angle_from_horizontal_plane > 0.01f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(diff2), yaw_adjustment_speed);
            yield return new WaitForFixedUpdate();
        }

        while (transform.position.y - default_pos_y < 1800)
        {
            yield return new WaitForFixedUpdate();
        }

        pattern = null;

    }

    IEnumerator Pattern201()//通常攻撃
    {
        float t = 0;
        while (t < 5)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(diff), yaw_adjustment_speed);
            moving_power = 1 / Mathf.Cos(angle_from_horizontal_plane * Mathf.Deg2Rad);

            if (esms.reloading)
            {
                break;
            }

            t += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        pattern = null;
    }

    void ChangePattern(IEnumerator next_pattern)
    {
        StopCoroutine(pattern);
        pattern = null;
        pattern = next_pattern;
        StartCoroutine(pattern);
    }
}
