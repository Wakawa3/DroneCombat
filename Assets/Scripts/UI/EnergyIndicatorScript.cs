using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyIndicatorScript : MonoBehaviour //各エネルギー消費よりも後に処理するとする（前でもいい）
{
    public float missile_energy = 100;
    public float bullet_energy = 100;

    float max_missile_energy = 100;
    float max_bullet_energy = 100;

    public float missile_energy_recovery_speed = 10;
    public float bullet_energy_recovery_speed = 10;

    [SerializeField] Image missile_energy_image = null;
    [SerializeField] Image bullet_energy_image = null;

    public bool gun_over_heat = false;
    float gun_over_heat_time = 0;
    public float gun_over_heat_recovery_time = 5;

    void Start()
    {
        
    }

    void Update()
    {
        missile_energy += missile_energy_recovery_speed * Time.deltaTime;
        if (missile_energy >= max_missile_energy) missile_energy = max_missile_energy;


        bullet_energy += bullet_energy_recovery_speed * Time.deltaTime;
        if (bullet_energy >= max_bullet_energy) bullet_energy = max_bullet_energy;
        else if (bullet_energy <= 0)
        {
            bullet_energy = 0;
            gun_over_heat = true;
        }

        if (gun_over_heat)
        {
            gun_over_heat_time += Time.deltaTime;
            if (gun_over_heat_time >= gun_over_heat_recovery_time)
            {
                gun_over_heat_time = 0;
                gun_over_heat = false;
            }
        }

        //描画部分
        missile_energy_image.fillAmount = missile_energy / max_missile_energy;
        bullet_energy_image.fillAmount = bullet_energy / max_bullet_energy;
        
        if (gun_over_heat) bullet_energy_image.color = new Color(255f / 255f, 223f / 255f, 0);
        else bullet_energy_image.color = new Color(20f / 255f, 226f / 255f, 74f / 255f);

    }
}
