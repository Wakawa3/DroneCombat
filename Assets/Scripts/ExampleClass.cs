using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour
{
   // public LayerMask mask;
    public GameObject player;

    GameObjectManagement gom;
    GameObject management;

    //GameObject alert_marker;
    //public GameObject alert_marker_p;

    void Start()
    {
        management = GameObject.FindWithTag("Management");
        gom = management.GetComponent<GameObjectManagement>();
    }
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            //mask = LayerMask.GetMask(new string[] { "Water", "UI" });

            //   alert_marker = Instantiate(alert_marker_p, gom.canvas.transform) as GameObject;
            // alert_marker.GetComponent<AlertDirectionMarkerScript>().enemy_missile = gameObject;
           

            Debug.Log("左");
        }
        if (Input.GetMouseButtonDown(1))
        {
            print("いま右ボタンが押された");
            //Destroy(alert_marker);
        }
        if (Input.GetMouseButtonDown(2))
        {
            print("いま中ボタンが押された");
        }
    }
}