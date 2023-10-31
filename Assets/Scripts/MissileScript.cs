using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ミサイルを検知するAIスクリプトより後に処理
public class MissileScript : MonoBehaviour
{
    public float speed = 0.5f;
    ShootingMissile sm;
    GameObject target;
    public float max_adjustment_angle; //度数法であることに注意
    float lock_on_degree;

    public float maxrange;
    Vector3 posBefore;
    float totalDistance;
    
    public float Damage;

    HitDetectionScript hds;

    bool processed = false;

    [HideInInspector] [SerializeField] GameObject explosion = null;

    Transform missile_partcle;
    MissileParticleScript mps;

    IDetectMissile idm;

    void Start()
    {
        sm = GameObjectManagement.player.GetComponent<ShootingMissile>();
        target = sm.Target;
        lock_on_degree = sm.lock_on_degree;


        posBefore = transform.position;


        hds = GameObjectManagement.player.GetComponent<HitDetectionScript>();

        missile_partcle = transform.GetChild(0);
        mps = missile_partcle.gameObject.GetComponent<MissileParticleScript>();

        if (target != null)
        {
            idm = target.GetComponent<IDetectMissile>();
        }
    }

    
    void FixedUpdate()
    {
        if (target != null)
        {
            if (lock_on_degree == 100)
            {
                Vector3 targetpos = target.transform.position;

                Vector3 diff = targetpos - transform.position;

                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(diff), max_adjustment_angle);

                float distance = Vector3.Distance(targetpos, transform.position);
                float angle = Vector3.Angle(diff, transform.forward);
                if (distance >= 100 && angle > 110)
                {
                    hds.GenerateHitDetection(HitDetectionScript.hit_detection_type.miss);
                    MissileDestroy();
                }

                if (idm != null)
                {
                    idm.DetectMissile();
                }
            }
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, speed + transform.localScale.z / 2))
        {
            if (hit.collider.gameObject.tag == "Enemy" && processed == false)
            {
                hit.collider.gameObject.GetComponent<EnemyHPScript>().EnemyHP -= Damage;

                if (hit.collider.gameObject.GetComponent<EnemyHPScript>().EnemyHP > 0)
                {
                    hds.GenerateHitDetection(HitDetectionScript.hit_detection_type.hit);
                }
                else
                {
                    hds.GenerateHitDetection(HitDetectionScript.hit_detection_type.destroyed);
                }

                Instantiate(explosion, transform.position, transform.rotation);
                StartCoroutine(NextframeMissileDestroy());
            }

            if (hit.collider.gameObject.tag == "EnemyChild" && processed == false)
            {
                hit.collider.gameObject.transform.root.gameObject.GetComponent<EnemyHPScript>().EnemyHP -= Damage;

                if (hit.collider.gameObject.transform.root.gameObject.GetComponent<EnemyHPScript>().EnemyHP > 0)
                {
                    hds.GenerateHitDetection(HitDetectionScript.hit_detection_type.hit);
                }
                else
                {
                    hds.GenerateHitDetection(HitDetectionScript.hit_detection_type.destroyed);
                }

                Instantiate(explosion, transform.position, transform.rotation);
                StartCoroutine(NextframeMissileDestroy());
            }

        }
        
        transform.position = transform.position + transform.forward * speed; //移動


        totalDistance = totalDistance + Vector3.Distance(posBefore, transform.position); //最大射程の計算
        posBefore = transform.position;

        if (totalDistance >= maxrange && processed == false)
        {
            hds.GenerateHitDetection(HitDetectionScript.hit_detection_type.miss);
            MissileDestroy();
        }
    }

    void OnTriggerEnter(Collider other)
    { 
        //同時に2つにふれると両方呼び出されるのでbool型processedで対策
        if (other.gameObject.tag == "Enemy" && processed == false)
        {
            other.gameObject.GetComponent<EnemyHPScript>().EnemyHP -= Damage;
            if (other.gameObject.GetComponent<EnemyHPScript>().EnemyHP > 0)
            {
                hds.GenerateHitDetection(HitDetectionScript.hit_detection_type.hit);
            }
            else
            {
                hds.GenerateHitDetection(HitDetectionScript.hit_detection_type.destroyed);
            }

            Instantiate(explosion, transform.position, transform.rotation);
            MissileDestroy();
        }

        if (other.gameObject.tag == "EnemyChild" && processed == false)
        {
            other.gameObject.transform.root.gameObject.GetComponent<EnemyHPScript>().EnemyHP -= Damage;
            if (other.gameObject.transform.root.gameObject.GetComponent<EnemyHPScript>().EnemyHP > 0)
            {
                hds.GenerateHitDetection(HitDetectionScript.hit_detection_type.hit);
            }
            else
            {
                hds.GenerateHitDetection(HitDetectionScript.hit_detection_type.destroyed);
            }

            Instantiate(explosion, transform.position, transform.rotation);
            MissileDestroy();
        }

        if (other.gameObject.tag == "Terrain" && processed == false)
        {
            hds.GenerateHitDetection(HitDetectionScript.hit_detection_type.miss);
            Instantiate(explosion, transform.position, transform.rotation);
            MissileDestroy();
        }

    }

    private void MissileDestroy()
    {
        processed = true;

        transform.DetachChildren();
        mps.missile_destroyed = true;

        Destroy(this.gameObject);
    }

    IEnumerator NextframeMissileDestroy()
    {
        processed = true;

        yield return new WaitForFixedUpdate();
        
        transform.DetachChildren();
        mps.missile_destroyed = true;

        Destroy(this.gameObject);
    } 

}