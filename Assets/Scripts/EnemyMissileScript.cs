using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//AlertDirectionMarkerScriptとの関係でAutoDestroyScriptとGivingDamageScriptをここにまとめた
//MissileAlertScriptより後に処理
public class EnemyMissileScript : MonoBehaviour 
{
    public float speed = 4;
    [HideInInspector] public GameObject launcher_holder;

    EnemyShootingMissileScript esms;

    MissileAlertScript mas;

    GameObject target;

    public float max_adjustment_angle = 0.65f;


    [HideInInspector] [SerializeField] GameObject alert_marker_prefab = null;
    GameObject alert_marker;


    public float maxrange; //AutoDestroyScript部分
    Vector3 posBefore;
    public float totalDistance;

    public float Damage; //GivingDamageScript部分

    bool processed = false;

    Transform missile_partcle;
    MissileParticleScript mps;

    [HideInInspector] [SerializeField] GameObject explosion = null;

    void Start()
    {
        esms = launcher_holder.GetComponent<EnemyShootingMissileScript>();

        mas = GameObjectManagement.player.GetComponent<MissileAlertScript>();
        mas.missile_number++;

        target = esms.target;

        if (target == GameObjectManagement.player)
        {
            alert_marker = Instantiate(alert_marker_prefab, GameObjectManagement.canvas.transform) as GameObject;
            alert_marker.GetComponent<AlertDirectionMarkerScript>().enemy_missile = gameObject;
        }


        posBefore = transform.position; //AutoDestroyScript部分

        missile_partcle = transform.GetChild(0);
        mps = missile_partcle.gameObject.GetComponent<MissileParticleScript>();
    }

    
    void FixedUpdate()
    {
        Vector3 targetpos = target.transform.position;

        Vector3 diff = targetpos - transform.position;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(diff), max_adjustment_angle);

        float distance = Vector3.Distance(targetpos, transform.position);
        float angle = Vector3.Angle(diff, transform.forward);
        if (distance >= 30 && angle > 90 && processed == false)
        {
            EnemyMissileDestroy();
        }

        transform.position = transform.position + transform.forward * speed;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, speed + transform.localScale.z / 2))
        {
            if (hit.collider.gameObject.tag == "Player" && processed == false)
            {    
                StartCoroutine(CollisionTargetDetect(hit.collider.gameObject, hit.point));
                
            }
        }

        totalDistance = totalDistance + Vector3.Distance(posBefore, transform.position); //AutoDestroyScript部分
        posBefore = transform.position;

        if (totalDistance >= maxrange && processed == false)
        {
            EnemyMissileDestroy();
        }
    }

    void OnTriggerEnter(Collider other) //GivingDamageScript部分
    {

        if (other.gameObject.tag == "Player" && processed == false)
        {
            other.gameObject.GetComponent<HPScript>().HP -= Damage;
            Instantiate(explosion, transform.position, transform.rotation);
            EnemyMissileDestroy();
        }

        if (other.gameObject.tag == "Terrain" && processed == false)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            EnemyMissileDestroy();
        }

    }

    void EnemyMissileDestroy()
    {
        if (target == GameObjectManagement.player)
        {
            Destroy(alert_marker);
        }

        processed = true;
        mas.missile_number--;
        transform.DetachChildren();
        mps.missile_destroyed = true;
        Destroy(this.gameObject);

    }

    IEnumerator CollisionTargetDetect(GameObject gm, Vector3 hit_point)
    {
        yield return new WaitForFixedUpdate();

        transform.position = hit_point;
        gm.GetComponent<HPScript>().HP -= Damage;
        Instantiate(explosion, transform.position, transform.rotation);
        EnemyMissileDestroy();
        
    }
}