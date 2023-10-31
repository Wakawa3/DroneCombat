using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherRotationScript : MonoBehaviour //EnemySearchTargetScriptよりも後に処理
{
    EnemySearchTargetScript ests;
    GameObject target;
    [SerializeField] float max_adjustment_angle = 0.4f;
    [SerializeField] float max_rotation_angle = 25;

    Quaternion body_to_launcher_default_qua;//以下SAMRotationScriptとの変化を//で示す

    Vector3 default_forward;

    float max_rotation_tan;

    [SerializeField] GameObject body = null;

    void Start()
    {
        
        if (transform.parent == null)
        {
            ests = GetComponent<EnemySearchTargetScript>();
        }
        else
        {
            ests = transform.root.GetComponent<EnemySearchTargetScript>();
        }

        body_to_launcher_default_qua.SetFromToRotation(body.transform.forward, transform.forward);//

        float b = 90 - max_rotation_angle; //xz平面からの角度
        max_rotation_tan = b * Mathf.Deg2Rad;
    }

    
    void Update()
    {
        default_forward = body.transform.rotation * body_to_launcher_default_qua * Vector3.forward;//

        target = ests.target;

        if (target != null)
        {
            Vector3 diff = target.transform.position - transform.position;

            RaycastHit hit;
            
            if (Physics.Raycast(transform.position, diff, out hit, Vector3.Magnitude(diff)) && hit.collider.gameObject.tag != "Terrain")
            {
                float angle = Vector3.Angle(default_forward, diff);

                if (angle <= max_rotation_angle)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(diff), max_adjustment_angle);
                }
                else
                {
                    Vector3 projected_direction = Vector3.ProjectOnPlane(diff, default_forward);
                    projected_direction = Vector3.Normalize(projected_direction);

                    Vector3 direction = projected_direction + default_forward * Mathf.Tan(max_rotation_tan);

                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), max_adjustment_angle);

                }
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.up), max_adjustment_angle);
            }
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.up), max_adjustment_angle);
        }

    }
}
