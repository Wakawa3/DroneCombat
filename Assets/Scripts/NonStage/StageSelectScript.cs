using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//TitleMenuScriptより後に処理
public class StageSelectScript : MenuClass
{
    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
        {
            if (choice_number <= menu_choice.Length - 2)
            {
                SceneManager.LoadScene("Stage" + (choice_number + 1));
            }

            if (choice_number == menu_choice.Length - 1)
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
