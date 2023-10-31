using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GivingDamageScript : MonoBehaviour
{

    public float Damage;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyHPScript>().EnemyHP -= Damage;
            Destroy(this.gameObject);
        }
    }
    
}
