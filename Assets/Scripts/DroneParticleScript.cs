using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneParticleScript : MonoBehaviour
{
    ParticleSystem ps;
    ParticleSystem.MainModule psm;
    ParticleSystem.EmissionModule pse;

    DroneAI dAI;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        psm = ps.main;
        pse = ps.emission;

        dAI = transform.root.gameObject.GetComponent<DroneAI>();
    }

    void Update()
    {
        if (dAI.moving_power <= 1)
        {
            psm.startSpeedMultiplier = 10 * Mathf.Lerp(1, dAI.moving_power, 0.5f);
            pse.rateOverTime = 10 * Mathf.Lerp(1, dAI.moving_power, 0.5f); ;
        }
        else
        {
            psm.startSpeedMultiplier = 10 * Mathf.Lerp(1, dAI.moving_power, 0.5f);
            pse.rateOverTime = 10 * Mathf.Lerp(1, dAI.moving_power, 0.1f);
        }
    }
}
