using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManagement : MonoBehaviour
{
    public static GameObject player, main_camera;
    public static GameObject canvas, canvas1;
    public static GameObject radar;

    [SerializeField] GameObject player_obj = null, main_camera_obj = null;
    [SerializeField] GameObject canvas_obj = null, canvas1_obj = null;
    [SerializeField] GameObject radar_obj = null;

    void Awake()
    {
        player = player_obj;
        main_camera = main_camera_obj;
        canvas = canvas_obj;
        canvas1 = canvas1_obj;
        radar = radar_obj;
    }
}
