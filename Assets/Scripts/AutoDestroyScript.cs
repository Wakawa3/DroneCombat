using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyScript : MonoBehaviour
{
    public float maxrange;
    Vector3 posBefore;
    float totalDistance;

    void Start()
    {
        posBefore = transform.position;
    }

    
    void Update()
    {
        totalDistance = totalDistance + Vector3.Distance(posBefore,transform.position);
        posBefore = transform.position;
        
        if(totalDistance >= maxrange)
        {
            Destroy(this.gameObject);
        }
    }
}
