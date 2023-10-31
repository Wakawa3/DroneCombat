using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDestroyScript : MonoBehaviour
{
    float t = 0;
    [SerializeField] float lifetime = 2;

    void Start()
    {
        
    }

    void Update()
    {
        t += Time.deltaTime;
        if (t > lifetime)
        {
            Destroy(this.gameObject);
        }
    }
}
