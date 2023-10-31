using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyRadarRenderingScript : MonoBehaviour
{
    [HideInInspector] public GameObject enemy;//EnemyApperanceで指定
    EnemyAppearance ea;

    GameObject player;
    GameObject radar;

    RectTransform my_rect_transform;
    RectTransform radar_rect_transform;

    public static float distance_divisor = 30;

    Image image;

    [SerializeField] Sprite sp_enemy_radar_marker = null;
    [SerializeField] Sprite sp_ground_enemy_radar_marker = null;
    [SerializeField] Sprite sp_outside_enemy_radar_marker = null;

    void Start()
    {
        player = GameObjectManagement.player;
        radar = GameObjectManagement.radar;
        my_rect_transform = GetComponent<RectTransform>();
        radar_rect_transform = radar.GetComponent<RectTransform>();

        image = GetComponent<Image>();

        ea = enemy.GetComponent<EnemyAppearance>();
    }

    
    void Update()
    {
        Vector2 enemy_pos2 = new Vector2(enemy.transform.position.x, enemy.transform.position.z);
        Vector2 player_pos2 = new Vector2(player.transform.position.x, player.transform.position.z);
        Vector2 diff2 = enemy_pos2 - player_pos2;
        Vector2 player_forward2 = new Vector2(player.transform.forward.x, player.transform.forward.z);
        float angle = Vector2.Angle(diff2, player_forward2);
        float angle_radian = angle * Mathf.Deg2Rad;

        Vector3 diff = enemy.transform.position - player.transform.position;
        Vector3 cross = Vector3.Cross(player.transform.forward, diff); //y成分をangleの正負の判定に使う

        float radius = radar_rect_transform.sizeDelta.x / 2; //レーダーが正円であることが前提

        float distance = Vector2.Distance(enemy_pos2, player_pos2);

        Vector2 radar_pos = radar_rect_transform.anchoredPosition;

        Vector2 enemy_direction;
        if (cross.y >= 0) enemy_direction = new Vector2(Mathf.Sin(angle_radian), Mathf.Cos(angle_radian));
        else enemy_direction = new Vector2(-Mathf.Sin(angle_radian), Mathf.Cos(angle_radian)); //Unityの座標系では外積した際の正負が通常と逆

        if (distance / distance_divisor <= radius - 4)
        {
            my_rect_transform.anchoredPosition = radar_pos + enemy_direction * distance / distance_divisor;

            if (ea.is_ground_enemy)
            {
                image.sprite = sp_ground_enemy_radar_marker;
                my_rect_transform.sizeDelta = new Vector2(4f, 4f);
            }
            else
            {
                image.sprite = sp_enemy_radar_marker;
                my_rect_transform.sizeDelta = new Vector2(6f, 6f);
            }

            Vector2 enemy_forward2 = new Vector2(enemy.transform.forward.x, enemy.transform.forward.z);
            float angle_a = Vector2.Angle(player_forward2, enemy_forward2);
            Vector3 cross_a = Vector3.Cross(player.transform.forward, enemy.transform.forward);

            if (cross_a.y >= 0) transform.rotation = Quaternion.Euler(0, 0, -angle_a);
            else transform.rotation = Quaternion.Euler(0, 0, angle_a);
        }
        else
        {
            my_rect_transform.anchoredPosition = radar_pos + enemy_direction * (radius - 4);
            
            image.sprite = sp_outside_enemy_radar_marker;
            my_rect_transform.sizeDelta = new Vector2(6f, 4f);

            if (cross.y >= 0) transform.rotation = Quaternion.Euler(0, 0, -angle);
            else transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
