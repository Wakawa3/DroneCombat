using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPGaugeScript : MonoBehaviour
{
    GameObject player;
    HPScript hps;

    Image gauge;

    Color green;
    Color red;

    void Start()
    {
        player = GameObjectManagement.player;
        hps = player.GetComponent<HPScript>();

        gauge = GetComponent<Image>();

        green = new Color(20f / 255f, 226f / 255f, 74f / 255f);
        red = new Color(1f, 37f / 255f, 28f / 255f);
    }

    void Update()
    {
        gauge.fillAmount = hps.HP / 100f;

        if (hps.HP > 35)
        {
            gauge.color = green;
        }
        else
        {
            gauge.color = red;
        }
    }
}
