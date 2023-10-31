using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageAreaScript : MonoBehaviour
{
    GameObject player;

    EventManagement evm;
    
    [SerializeField] RectTransform stage_area_back = null;
    [SerializeField] RectTransform stage_area_mask = null;

    [SerializeField] int area_event_number = 1;
    [SerializeField] Vector3 stage_center = Vector3.zero;
    [SerializeField] float stage_radius = 5000;

    Vector2 center_pos2;

    void Start()
    {
        player = GameObjectManagement.player;

        stage_area_back.sizeDelta = new Vector2(stage_radius * 2 / EnemyRadarRenderingScript.distance_divisor, stage_radius * 2 / EnemyRadarRenderingScript.distance_divisor);
        stage_area_mask.sizeDelta = new Vector2(stage_area_back.sizeDelta.x - 2, stage_area_back.sizeDelta.y - 2);

        center_pos2 = new Vector2(stage_center.x, stage_center.z);

        evm = GetComponent<EventManagement>();
    }

    void Update()
    {       
        Vector2 player_pos2 = new Vector2(player.transform.position.x, player.transform.position.z);
        Vector2 diff2 = center_pos2 - player_pos2;
        Vector2 player_forward2 = new Vector2(player.transform.forward.x, player.transform.forward.z);
        float angle = Vector2.Angle(diff2, player_forward2);
        float angle_radian = angle * Mathf.Deg2Rad;

        Vector3 diff = stage_center - player.transform.position;
        Vector3 cross = Vector3.Cross(player.transform.forward, diff); //y成分をangleの正負の判定に使う
        
        float distance = Vector2.Distance(center_pos2, player_pos2);

        Vector2 center_direction;
        if (cross.y >= 0) center_direction = new Vector2(Mathf.Sin(angle_radian), Mathf.Cos(angle_radian));
        else center_direction = new Vector2(-Mathf.Sin(angle_radian), Mathf.Cos(angle_radian)); //Unityの座標系では外積した際の正負が通常と逆

        stage_area_back.anchoredPosition = center_direction * distance / EnemyRadarRenderingScript.distance_divisor;
        stage_area_mask.anchoredPosition = stage_area_back.anchoredPosition;


        if (Vector3.Magnitude(diff2) > stage_radius && evm.event_processed[area_event_number] == false)
        {
            evm.RunEventMethod(area_event_number);
        }
    }
}
