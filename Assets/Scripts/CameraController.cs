using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour//他のスクリプトよりも早く処理
{
    GameObject player;
    Vector3 a,up;
    float hp;
    float hp_before;

    Vector3 vibration_vector = Vector3.zero;

    void Start()
    {
        player = GameObjectManagement.player;

        hp_before = player.GetComponent<HPScript>().HP;
    }

    void Update()
    { 
        hp = player.GetComponent<HPScript>().HP;

        if (hp != hp_before)
        {
            //float diff_hp = hp_before - hp;
            IEnumerator make_vibration = MakeVibration();
            StartCoroutine(make_vibration);
        }

        transform.position = player.transform.position + (player.transform.forward * -10f) + (transform.up * 3f) + vibration_vector;
        transform.forward = player.transform.forward;

        hp_before = hp;
    }

    IEnumerator MakeVibration(/*float diff_hp*/)
    {
        for (int i = 0; i < 30; i++)
        {
            vibration_vector = Random.onUnitSphere * 0.15f;
            vibration_vector = Vector3.ProjectOnPlane(vibration_vector, transform.forward);
            yield return null;
        }
        vibration_vector = Vector3.zero;
    }
}
