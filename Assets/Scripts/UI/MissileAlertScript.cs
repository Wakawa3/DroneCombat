using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileAlertScript : MonoBehaviour
{
    public bool cation;
    public int missile_number = 0;

    [SerializeField] Image cation_back = null;
    [SerializeField] Text cation_text = null;
    [SerializeField] Image alert_back = null;
    [SerializeField] Text alert_text = null;
    
    void Update()
    {
        if (missile_number >= 1)
        {
            alert_back.color = new Color(255f / 255f, 37f / 255f, 28f / 255f, 1f);
            alert_text.color = new Color(0, 0, 0, 1);
        }
        else
        {
            alert_back.color = new Color(0, 0, 0, 0);
            alert_text.color = new Color(0, 0, 0, 0);
        }

        if (cation)
        {
            cation_back.color = new Color(232f / 255f, 238f / 255f, 44f / 255f, 1f);
            cation_text.color = new Color(0, 0, 0, 1);
        }
        else
        { 
            cation_back.color = new Color(0, 0, 0, 0);
            cation_text.color = new Color(0, 0, 0, 0);
        }
        cation = false;
    }
}
