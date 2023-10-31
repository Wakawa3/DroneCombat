using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//必ず親オブジェクトに付ける
public class EnemySearchTargetScript : MonoBehaviour //EnemyShootingMissileScriptより先に処理
{
    [SerializeField] float radius = 1500;
    LayerMask mask;
    [HideInInspector] public GameObject target = null;

    [HideInInspector] public bool external_target_change = false;

    void Start()
    {
        mask = LayerMask.GetMask("Player");
    }

    void FixedUpdate()
    {
        if (external_target_change)
        {
            return;
        }

        if (target == null)
        {
            SearchTarget();
        }
        else
        {
            bool target_lost = true;
            Collider[] targets = Physics.OverlapSphere(transform.position, radius, mask);

            for (int i = 0; i < targets.Length; i++)
            {
                if (target == targets[i])
                {
                    target_lost = false;
                }
            }

            if (target_lost)
            {
                SearchTarget();
            }
        }

    }

    void SearchTarget()
    {
        target = null;
        Collider[] targets = Physics.OverlapSphere(transform.position, radius, mask); //索敵範囲のコライダーを取得
        float[] targets_distance = new float[targets.Length];

        float min_distance = radius;

        for (int i = 0; i < targets.Length; i++)
        {
            targets_distance[i] = Vector3.Distance(targets[i].gameObject.transform.position, transform.position);

            if (targets_distance[i] < min_distance)
            {
                min_distance = targets_distance[i];
                target = targets[i].gameObject;
            }
        }
    }
}
