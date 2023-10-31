using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//EnemyMissileScriptにて生成
public class AlertDirectionMarkerScript : MonoBehaviour
{
    public GameObject enemy_missile;//EnemyMissileScriptで変更

    GameObject player;
    Color yellow, red;
    Image image;

    float yellow_border_distance = 300;
    float red_border_distance = 150;

    void Start()
    {
        player = GameObjectManagement.player;

        yellow = new Color(255f / 255f, 223f / 255f, 0);
        red = new Color(255f / 255f, 37f / 255f, 28f / 255f);
        image = GetComponent<Image>();
    }

    
    void Update()
    {
        //マーカーの位置
        Vector3 diff = enemy_missile.transform.position - player.transform.position;
        Vector2 diff2 = new Vector2(diff.x, diff.z);
        Vector2 forward2 = new Vector2(player.transform.forward.x, player.transform.forward.z);

        float angle = Vector2.Angle(forward2, diff2);

        Vector3 cross = Vector3.Cross(player.transform.forward, diff);

        if (cross.y >= 0) transform.rotation = Quaternion.Euler(45, 0, -angle); //Unityの座標系では角度の正と負はこうなる
        else transform.rotation = Quaternion.Euler(45, 0, angle); //Quaternion.Eulerは反時計回りが正


        //マーカーの色
        float distance = Vector3.Magnitude(diff);

        if (distance >= yellow_border_distance)
        {
            image.color = yellow;
        }
        else if (distance >= red_border_distance)
        {
            float rate = (distance - red_border_distance) / (yellow_border_distance - red_border_distance);
            image.color = Color.Lerp(red, yellow, rate);
        }
        else
        {
            image.color = red;
        }

    }
}
