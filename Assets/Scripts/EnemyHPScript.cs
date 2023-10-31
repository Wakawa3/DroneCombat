using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHPScript : MonoBehaviour //EnemyRadarRenderingScriptよりも早く処理
{
    public float EnemyHP;

    GameObject management;
    EnemyManagement em;
    GameObject player;

    EnemyAppearance ea;
    

    [SerializeField] int score = 0;

    [SerializeField] GameObject explosion = null;

    void Start()
    {
        management = GameObject.FindWithTag("Management");
        em = management.GetComponent<EnemyManagement>();
        
        player = GameObjectManagement.player;

        ea = GetComponent<EnemyAppearance>();
        
    }


    void FixedUpdate()
    {
        if (EnemyHP <= 0f)
        {
            ScoreScript.score += score;

            DestroyEnemy();

            Instantiate(explosion, transform.position, transform.rotation);
        }

    }

    public void DestroyEnemy()
    {
        em.EnemyObject[ea.EnemyObjectID] = null;
        em.number_of_enemies--;
        if (ea.is_target == true)
        {
            em.number_of_targets--;
        }

        Destroy(ea.enemy_radar_marker);

        Destroy(gameObject);
    }
}
