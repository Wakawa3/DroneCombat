using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleScript : MonoBehaviour
{
    ParticleSystem ps;
    ParticleSystem.MainModule psm;
    ParticleSystem.EmissionModule pse;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        psm = ps.main;
        pse = ps.emission;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            psm.startSpeedMultiplier = 5;
            pse.rateOverTime = 5;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            psm.startSpeedMultiplier = 15;
            pse.rateOverTime = 11;
        }
        else
        {
            psm.startSpeedMultiplier = 10;
            pse.rateOverTime = 10;
        }
    }
}