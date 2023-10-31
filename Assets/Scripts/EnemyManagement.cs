using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagement : MonoBehaviour
{
    public GameObject[] EnemyObject = new GameObject[0];
    public float[] angle_from_camera_to_enemy = new float[0];
    public bool[] enemy_is_rendered = new bool[0];
    public Vector2[] enemy_screen_pos = new Vector2[0];
    [HideInInspector] public int number_of_enemies = 0;
    [HideInInspector] public int number_of_targets = 0;
    
    void Start()
    {
        
    }

  
    void Update()
    {
        for (int i = 0; i < EnemyObject.Length; i++)
        {
            if (EnemyObject[i] != null)
            {
                Vector3 diff = EnemyObject[i].transform.position - GameObjectManagement.main_camera.transform.position;
                angle_from_camera_to_enemy[i] = Vector3.Angle(diff, GameObjectManagement.main_camera.transform.forward);

                if (angle_from_camera_to_enemy[i] < 90)
                {
                    enemy_is_rendered[i] = true;
                    enemy_screen_pos[i] = RectTransformUtility.WorldToScreenPoint(Camera.main, EnemyObject[i].transform.position);
                }
                else
                {
                    enemy_is_rendered[i] = false;
                    enemy_screen_pos[i] = new Vector2(0, 0); //注意　enemy_is_rendered = falseの時は絶対に使わない
                }
            }
            else
            {
                angle_from_camera_to_enemy[i] = 1000; //オブジェクトがなければ1000
                enemy_is_rendered[i] = false;
                enemy_screen_pos[i] = new Vector2(0, 0); //注意　enemy_is_rendered = falseの時は絶対に使わない
            }


        }
    }
}
