using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TitleMenuScript : MenuClass
{
    [SerializeField] GameObject stage_select_prefab = null;
    [SerializeField] GameObject record_prefab = null;

    public static int reserved_choice_number = -1; //他シーンから移るとき自動的に処理するメニュー 処理しないときは-1

    new void Start()
    {
        base.Start();
        
        if (reserved_choice_number == 0)
        {
            reserved_choice_number = -1;
            activated = false;
            Instantiate(stage_select_prefab, transform);
        }
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
            if (choice_number == 0)//StageSelect
            {
                activated = false;
                Instantiate(stage_select_prefab, transform);
            }

            if (choice_number == 1)
            {
                activated = false;
                Instantiate(record_prefab, transform);
            }

            if (choice_number == 2)//Exit
            {

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
                Application.Quit();
#endif
            }
        }
    }
}
