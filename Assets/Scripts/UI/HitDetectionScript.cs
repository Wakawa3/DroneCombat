using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitDetectionScript : MonoBehaviour
{
    GameObject canvas1;

    [SerializeField] GameObject hit_detection_prefab = null;
    GameObject[] hit_detection_object = new GameObject[3]; //とりあえず３にした
                                                           // RectTransform[] hit_detection_object_rect_transform = new RectTransform[3];
    float[] rendered_time = new float[] { 0, 0, 0 };
    float height;
    Vector2 default_pos;

    [HideInInspector] public GameObject gun_hit_detection_object = null;
    GameObject gun_destroyed_detection_object = null;
    int gun_hit = 0;
    float gun_hit_rendered_time = 0;
    float gun_destroyed_rendered_time = 0;

    const float max_rendered_time = 7;

    public enum hit_detection_type : int
    {
        hit,
        destroyed,
        miss
    }

    void Start()
    {
        canvas1 = GameObjectManagement.canvas1;

        height = hit_detection_prefab.GetComponent<RectTransform>().sizeDelta.y;
        default_pos = hit_detection_prefab.GetComponent<RectTransform>().anchoredPosition;
    }

    
    void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            if (hit_detection_object[i] != null)
            {
                rendered_time[i] += Time.deltaTime;
                if (rendered_time[i] > max_rendered_time)
                {
                    Destroy(hit_detection_object[i]);
                    hit_detection_object[i] = null;
                    rendered_time[i] = 0;
                }
            }
        }

        if (gun_hit_detection_object != null)
        {
            gun_hit_rendered_time += Time.deltaTime;
            if (gun_hit_rendered_time > max_rendered_time)
            {
                Destroy(gun_hit_detection_object);
                gun_hit_detection_object = null;
                gun_hit_rendered_time = 0;
                gun_hit = 0;
            }
        }

        if (gun_destroyed_detection_object != null)
        {
            gun_destroyed_rendered_time += Time.deltaTime;
            if (gun_destroyed_rendered_time > max_rendered_time)
            {
                Destroy(gun_destroyed_detection_object);
                gun_destroyed_detection_object = null;
                gun_destroyed_rendered_time = 0;
            }
        }
    }

    public void GenerateHitDetection(hit_detection_type type)
    {
        Destroy(hit_detection_object[2]);

        hit_detection_object[2] = hit_detection_object[1];
        rendered_time[2] = rendered_time[1];

        if (hit_detection_object[2] != null)
        {
            hit_detection_object[2].GetComponent<RectTransform>().anchoredPosition = default_pos - new Vector2(0, (height + 4) * 2);
        }

        hit_detection_object[1] = hit_detection_object[0];
        rendered_time[1] = rendered_time[0];
        rendered_time[0] = 0;

        if (hit_detection_object[1] != null)
        {
            hit_detection_object[1].GetComponent<RectTransform>().anchoredPosition = default_pos - new Vector2(0, (height + 4) * 1);
        }

        hit_detection_object[0] = Instantiate(hit_detection_prefab, canvas1.transform) as GameObject;

        Text detection_text = hit_detection_object[0].transform.GetChild(0).gameObject.GetComponent<Text>();

        switch (type)
        {
            case hit_detection_type.hit:
                detection_text.text = "HIT";
                break;
            case hit_detection_type.destroyed:
                detection_text.text = "DESTROYED";
                hit_detection_object[0].GetComponent<Image>().color = new Color(0, 179f / 255f, 255f / 255f);
                break;
            case hit_detection_type.miss:
                detection_text.text = "MISS";
                hit_detection_object[0].GetComponent<Image>().color = new Color(255f / 255f, 223f / 255f, 0);
                break;
        }
            
    }

    public void GenerateGunHitDetection(hit_detection_type type)
    {
        switch (type)
        {
            case hit_detection_type.hit:
                gun_hit += 1;
                gun_hit_rendered_time = 0;

                if (gun_hit_detection_object == null)
                { 
                    gun_hit_detection_object = Instantiate(hit_detection_prefab, canvas1.transform) as GameObject;
                    gun_hit_detection_object.GetComponent<RectTransform>().anchoredPosition = default_pos - new Vector2(0, (height + 4) * 3 + 10);
                }

                gun_hit_detection_object.transform.GetChild(0).gameObject.GetComponent<Text>().text = "HIT × " + gun_hit;

                
                break;

            case hit_detection_type.destroyed:
                gun_destroyed_rendered_time = 0;

                if (gun_destroyed_detection_object == null)
                {
                    gun_destroyed_detection_object = Instantiate(hit_detection_prefab, canvas1.transform) as GameObject;
                    gun_destroyed_detection_object.GetComponent<RectTransform>().anchoredPosition = default_pos - new Vector2(0, (height + 4) * 4 + 10);
                    gun_destroyed_detection_object.GetComponent<Image>().color = new Color(0, 179f / 255f, 255f / 255f);
                    gun_destroyed_detection_object.transform.GetChild(0).gameObject.GetComponent<Text>().text = "DESTROYED";
                }
                break;
        }
        
    }
}
