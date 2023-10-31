using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PauseScriptより後に処理
public class OptionScript : MenuClass
{
    [SerializeField] GameObject adjust_object = null;
    AdjustSensitivityScript ass;

    new void Start()
    {
        base.Start();

        ass = adjust_object.GetComponent<AdjustSensitivityScript>();
    }

    new void Update()
    {
        base.Update();
        
        if (activated == false)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
        {
            if (choice_number == 1)
            {
                activated = false;
                ass.activated = true;
            }

            if (choice_number == 2)
            { 
                BackToBaseMenu();
            }
        }

        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Backspace))
        {
            BackToBaseMenu();
        }
    }
}
