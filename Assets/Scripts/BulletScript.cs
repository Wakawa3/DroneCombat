using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{   
    [HideInInspector] public float speed;
    public float Damage;

    GameObject player;
    HitDetectionScript hds;

    bool processed = false;

    void Start()
    {
        player = GameObjectManagement.player;
        hds = player.GetComponent<HitDetectionScript>();
    }

    
    void FixedUpdate()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, speed + transform.localScale.z / 2))
        {
         /*   if (hit.collider.gameObject.tag == "Enemy" && processed == false)
            {
                hit.collider.gameObject.GetComponent<EnemyHPScript>().EnemyHP -= Damage;

                hds.GenerateGunHitDetection(HitDetectionScript.hit_detection_type.hit);
                if (hit.collider.gameObject.GetComponent<EnemyHPScript>().EnemyHP <= 0)
                {
                    hds.GenerateGunHitDetection(HitDetectionScript.hit_detection_type.destroyed);
                }

                processed = true;
                Destroy(this.gameObject);
            }

            if (hit.collider.gameObject.tag == "EnemyChild" && processed == false)
            {
                hit.collider.gameObject.transform.root.gameObject.GetComponent<EnemyHPScript>().EnemyHP -= Damage;

                hds.GenerateGunHitDetection(HitDetectionScript.hit_detection_type.hit);
                if (hit.collider.gameObject.transform.root.gameObject.GetComponent<EnemyHPScript>().EnemyHP <= 0)
                {
                    hds.GenerateGunHitDetection(HitDetectionScript.hit_detection_type.destroyed);
                }

                processed = true;
                Destroy(this.gameObject);
            }
            */
            if (hit.collider.gameObject.tag == "Terrain" && processed == false)
            {
                IEnumerator terrain_collision = TerrainCollision();
                StartCoroutine(terrain_collision);
            }

        }

        transform.position = transform.position + transform.forward * speed;
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Enemy" && processed == false)
        {
            other.gameObject.GetComponent<EnemyHPScript>().EnemyHP -= Damage;

            hds.GenerateGunHitDetection(HitDetectionScript.hit_detection_type.hit);
                if (other.gameObject.GetComponent<EnemyHPScript>().EnemyHP <= 0)
                {
                    hds.GenerateGunHitDetection(HitDetectionScript.hit_detection_type.destroyed);
                }

            //Instantiate(explosion, other.transform.position, Quaternion.identity);

            processed = true;
            Destroy(this.gameObject);
        }

        if (other.gameObject.tag == "EnemyChild" && processed == false)
        {
            other.gameObject.transform.root.gameObject.GetComponent<EnemyHPScript>().EnemyHP -= Damage;

            hds.GenerateGunHitDetection(HitDetectionScript.hit_detection_type.hit);
            if (other.gameObject.transform.root.gameObject.GetComponent<EnemyHPScript>().EnemyHP <= 0)
            {
                hds.GenerateGunHitDetection(HitDetectionScript.hit_detection_type.destroyed);
            }

            //Instantiate(explosion, other.transform.position, Quaternion.identity);

            processed = true;
            Destroy(this.gameObject);
        }

    }

    IEnumerator TerrainCollision()
    {
        yield return new WaitForFixedUpdate();

        processed = true;
        Destroy(this.gameObject);

        
    }
}
