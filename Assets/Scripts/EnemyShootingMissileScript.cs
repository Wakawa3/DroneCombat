using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootingMissileScript : MonoBehaviour
{
    MissileAlertScript mas;

    EnemySearchTargetScript ests;

    [SerializeField] GameObject launcher = null;
    [SerializeField] bool multi_launcher = false;
    [SerializeField] GameObject[] multi_launcher_object = new GameObject[0];
    int next_launcher = 0;
    [HideInInspector] public GameObject target = null;

    [SerializeField] float max_lock_on_range = 800;
    [SerializeField] float searching_angle = 70;
    [SerializeField] float lock_on_power = 2;
    float lock_on_degree = 0;

    public GameObject enemy_missile_prefab;
    GameObject enemy_missile;
    public float time_to_launch = 3;
    float time_from_lock = 0;
    [HideInInspector] public bool reloading = false;//AIスクリプトで使用
    public float reload_time = 6;
    float time_from_launch = 0;

    LayerMask mask;

    [SerializeField] float missile_speed = 4;
    [SerializeField] float missile_max_adjustment_angle = 0.65f;

    void Start()
    {
        mas = GameObjectManagement.player.GetComponent<MissileAlertScript>();

        if (transform.parent == null)
        {
            ests = GetComponent<EnemySearchTargetScript>();
        }
        else
        {
            ests = transform.root.GetComponent<EnemySearchTargetScript>();
        }

        mask = LayerMask.GetMask("Player", "Terrain");
    }


    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        if (reloading)
        {
            time_from_launch += Time.deltaTime;
            if (time_from_launch >= reload_time)
            {
                time_from_launch = 0;
                reloading = false;
            }
        }

        target = ests.target;

        if (multi_launcher == false)
        {
            if (target != null)
            {
                Vector3 diff = target.transform.position - launcher.transform.position;

                if (Vector3.Magnitude(diff) <= max_lock_on_range)
                {
                    RaycastHit hit;

                    if (Physics.Raycast(launcher.transform.position, diff, out hit, max_lock_on_range, mask) && hit.collider.gameObject.tag != "Terrain")
                    {
                        float angle = Vector3.Angle(transform.forward, diff);

                        if (angle <= searching_angle)
                        {
                            lock_on_degree += lock_on_power * Time.deltaTime * 60;
                            if (lock_on_degree < 0) lock_on_degree = 0;
                            if (lock_on_degree > 100) lock_on_degree = 100;

                            time_from_lock += Time.deltaTime;

                            if (time_from_lock >= time_to_launch && reloading == false)
                            {
                                time_from_lock = 0;

                                enemy_missile = Instantiate(enemy_missile_prefab, launcher.transform.position, transform.rotation) as GameObject;
                                EnemyMissileScript ems = enemy_missile.GetComponent<EnemyMissileScript>();
                                ems.launcher_holder = gameObject;
                                ems.speed = missile_speed;
                                ems.max_adjustment_angle = missile_max_adjustment_angle;

                                reloading = true;
                            }

                            mas.cation = true;
                        }
                        else
                        {
                            lock_on_degree = 0;
                            time_from_lock = 0;
                        }

                    }
                    else
                    {
                        lock_on_degree = 0;
                        time_from_lock = 0;
                    }
                }
                else
                {
                    lock_on_degree = 0;
                    time_from_lock = 0;
                }

            }
            else
            {
                lock_on_degree = 0;
                time_from_lock = 0;
            }

        }
        else
        {
            if (target != null)
            {
                Vector3 diff = target.transform.position - transform.position;

                if (Vector3.Magnitude(diff) <= max_lock_on_range)
                {
                    RaycastHit hit;

                    if (Physics.Raycast(transform.position, diff, out hit, max_lock_on_range, mask) && hit.collider.gameObject.tag != "Terrain")
                    {
                        float angle = Vector3.Angle(transform.forward, diff);

                        if (angle <= searching_angle)
                        {
                            lock_on_degree += lock_on_power * Time.deltaTime * 60;
                            if (lock_on_degree < 0) lock_on_degree = 0;
                            if (lock_on_degree > 100) lock_on_degree = 100;

                            time_from_lock += Time.deltaTime;

                            if (time_from_lock >= time_to_launch && reloading == false)
                            {
                                time_from_lock = 0;

                                enemy_missile = Instantiate(enemy_missile_prefab, multi_launcher_object[next_launcher].transform.position, transform.rotation) as GameObject;

                                next_launcher++;
                                if (next_launcher >= multi_launcher_object.Length)
                                {
                                    next_launcher = 0;
                                }

                                EnemyMissileScript ems = enemy_missile.GetComponent<EnemyMissileScript>();
                                ems.launcher_holder = gameObject;
                                ems.speed = missile_speed;
                                ems.max_adjustment_angle = missile_max_adjustment_angle;

                                reloading = true;
                            }

                            mas.cation = true;
                        }
                        else
                        {
                            lock_on_degree = 0;
                            time_from_lock = 0;
                        }

                    }
                    else
                    {
                        lock_on_degree = 0;
                        time_from_lock = 0;
                    }
                }
                else
                {
                    lock_on_degree = 0;
                    time_from_lock = 0;
                }

            }
            else
            {
                lock_on_degree = 0;
                time_from_lock = 0;
            }

        }

    }

}
