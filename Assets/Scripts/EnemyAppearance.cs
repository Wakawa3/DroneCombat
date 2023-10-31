using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyAppearance : MonoBehaviour
{
    GameObject management;
    [HideInInspector] public int EnemyObjectID = -1; //デフォルトは-1
    EnemyManagement em;
    [HideInInspector] [SerializeField] GameObject lock_on_cursor = null;
    [HideInInspector] [SerializeField] GameObject enemy_radar_marker_prefab = null;
    [HideInInspector] public GameObject enemy_radar_marker;
    public bool is_ground_enemy = false;
    public bool is_target;

    void Start()
    {
        management = GameObject.FindWithTag("Management");
        em = management.GetComponent<EnemyManagement>();
        
        AppearanceMethod();
        
        GameObject cursor = Instantiate(lock_on_cursor, GameObjectManagement.canvas.transform) as GameObject;
        
        cursor.GetComponent<LockOnCursorScript>().enemyID = EnemyObjectID;


        enemy_radar_marker = Instantiate(enemy_radar_marker_prefab, GameObjectManagement.canvas1.transform) as GameObject;

        enemy_radar_marker.GetComponent<EnemyRadarRenderingScript>().enemy = gameObject;
    }

    void AppearanceMethod()
    {
        gameObject.SetActive(true);

        Array.Resize(ref em.EnemyObject, em.EnemyObject.Length + 1);

        Array.Resize(ref em.angle_from_camera_to_enemy, em.EnemyObject.Length);
        Array.Resize(ref em.enemy_is_rendered, em.EnemyObject.Length);
        Array.Resize(ref em.enemy_screen_pos, em.EnemyObject.Length);

        EnemyObjectID = em.EnemyObject.Length - 1;
        em.EnemyObject[EnemyObjectID] = gameObject;
        em.number_of_enemies++;
        if (is_target)
        {
            em.number_of_targets++;
        }
    }
}
