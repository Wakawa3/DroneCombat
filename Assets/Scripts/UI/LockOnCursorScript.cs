using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//EnemyAppearanceで生成
public class LockOnCursorScript : MonoBehaviour
{
    public int enemyID;
    RectTransform my_rt;

    GameObject management;
    EnemyManagement em;
    GameObject tracking_target;

    ShootingMissile sm;

    EnemyAppearance ea;

    [HideInInspector] [SerializeField] GameObject target_text_prefab = null;
    GameObject target_text = null;
    RectTransform target_text_rt;

    Image cursor;
    [SerializeField] Sprite cursor_sprite = null;

    [HideInInspector] [SerializeField] GameObject distance_text_prefab = null;
    GameObject distance_text = null;
    RectTransform distance_text_rt;
    Text distance_text_text;

    void Start()
    {
        management = GameObject.FindWithTag("Management");

        em = management.GetComponent<EnemyManagement>();
        tracking_target = em.EnemyObject[enemyID];

        my_rt = GetComponent<RectTransform>();

        sm = GameObjectManagement.player.GetComponent<ShootingMissile>();

        ea = tracking_target.GetComponent<EnemyAppearance>();

        cursor = GetComponent<Image>();
        cursor.sprite = cursor_sprite;
    }

   
    void Update()
    {
        if (tracking_target != null)
        {
            if (em.enemy_is_rendered[enemyID]) //敵が画面に映っているか
            {
                if (enemyID == sm.targetID) //ロックオンされているか
                {
                    if (distance_text == null && tracking_target != null)//後者はFixedUpdateとのずれを考慮
                    {
                        distance_text = Instantiate(distance_text_prefab, GameObjectManagement.canvas.transform) as GameObject;

                        distance_text_rt = distance_text.GetComponent<RectTransform>();
                        distance_text_text = distance_text.GetComponent<Text>();
                    }

                    distance_text_rt.anchoredPosition = my_rt.anchoredPosition + new Vector2(20, 7);

                    int displayed_distance = Mathf.CeilToInt(Vector3.Distance(tracking_target.transform.position, GameObjectManagement.player.transform.position));
                    distance_text_text.text = "" + displayed_distance;


                    if (sm.lock_on_degree == 100) cursor.color = new Color(255f / 255f, 37f / 255f, 28f / 255f, 1f);
                    else cursor.color = new Color(232f / 255f, 238f / 255f, 44f / 255f, 1f);
                }
                else
                {
                    if (distance_text != null)
                    {
                        Destroy(distance_text);
                        distance_text = null;
                    }

                    cursor.color = new Color(20f / 255f, 226f / 255f, 74f / 255f);
                }
                
                cursor.color = new Color(cursor.color.r, cursor.color.g, cursor.color.b, 1.0f);

                my_rt.position = em.enemy_screen_pos[enemyID];


                if (ea.is_target == true)
                {
                    if (target_text == null)
                    {
                        target_text = Instantiate(target_text_prefab, GameObjectManagement.canvas.transform) as GameObject;
                        target_text_rt = target_text.GetComponent<RectTransform>();
                    }

                    target_text_rt.anchoredPosition = my_rt.anchoredPosition + new Vector2(-20, 7);
                }
            }
            else
            {
                cursor.color = new Color(cursor.color.r, cursor.color.g, cursor.color.b, 0f);
                //カメラのちょうど裏側のスクリーン座標は前側のスクリーン座標と同じ
                //透明にしないと敵がカメラのちょうど内側にいるときカーソルが出てしまう
            }
        }
        else
        {
            if (target_text != null)
            {
                Destroy(target_text);
            }

            if (distance_text != null)
            {
                Destroy(distance_text);
                distance_text = null;
            }

            Destroy(this.gameObject);
        }
    }

    void TransferSubText(RectTransform rt, float by_x, float by_y)
    {
        rt.anchoredPosition = my_rt.anchoredPosition + new Vector2(by_x, by_y);
    }

}
