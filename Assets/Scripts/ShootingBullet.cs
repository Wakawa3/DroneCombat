using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBullet : MonoBehaviour
{
    [SerializeField] GameObject bullet = null;
    [SerializeField] Transform MuzzleL = null, MuzzleR = null;
    [SerializeField] float speed = 60f;
    float timecount = 0f;
    [SerializeField] float second_per_fire  = 0.067f;

    GameObject bullets;

    EnergyIndicatorScript eis;
    public float required_energy;

    void Start()
    {
        eis = GetComponent<EnergyIndicatorScript>();
    }

    void Update()
    {
        if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.Semicolon))
        {
            if (timecount < second_per_fire)
            { 
                timecount += Time.deltaTime;
                if (timecount >= second_per_fire)
                {
                    Firing(MuzzleL, timecount - second_per_fire);
                }
                    
            }
            else if (timecount >= second_per_fire)
            {
                timecount += Time.deltaTime;
                if (timecount >= 2 * second_per_fire)
                {
                    timecount = timecount - 2 * second_per_fire;
                    Firing(MuzzleR, timecount);
                }
            }

        }
        else
        {
            timecount = 0f;
        }
    }

    void Firing(Transform muzzle, float passed_time)
    {
        if (eis.gun_over_heat == false)
        {
            bullets = Instantiate(bullet, muzzle.position/* + transform.forward * passed_time / Time.fixedDeltaTime * speed*/, transform.rotation) as GameObject;
            bullets.GetComponent<BulletScript>().speed = speed;
            bullets.transform.forward = bullets.transform.forward + 0.015f * Random.onUnitSphere; //弾のばらつき

            eis.bullet_energy -= required_energy;
        }
    }
}