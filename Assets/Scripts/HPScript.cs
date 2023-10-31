using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPScript : MonoBehaviour
{
    public float HP;
    [SerializeField] GameObject explosion = null;

    GameObject management;
    EventManagement evm;
    [SerializeField] int game_over_event_number = 0;

    [SerializeField] Text HPtext = null;

    void Start()
    {
        management = GameObject.FindWithTag("Management");
        evm = management.GetComponent<EventManagement>();
    }

    void Update()
    {
        float displayed_HP;
        displayed_HP = Mathf.Ceil(HP);
        HPtext.text = "HP:" + displayed_HP;

        if (HP <= 0)
        {
            Instantiate(explosion, transform.position, transform.rotation);

            evm.RunEventMethod(game_over_event_number);

            gameObject.SetActive(false);
        }
    }
}
