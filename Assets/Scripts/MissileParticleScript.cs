using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileParticleScript : MonoBehaviour
{
    ParticleSystem ps;
    ParticleSystem.EmissionModule emission;
    [SerializeField] public bool missile_destroyed = false;

    float t = 0;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        emission = ps.emission;
    }

    
    void Update()
    {
        if (missile_destroyed)
        {
            emission.rateOverTime = 0;
            t += Time.deltaTime;
            if (t >= 15)
            {
                Destroy(this.gameObject);
            }

        }
    }
}