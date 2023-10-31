using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenuScript : MenuClass
{

    new void Start()
    {
        base.Start();

        Time.timeScale = 0;
    }

    new void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
        {
            if (choice_number == 0)//Restart Mission
            {
                Scene active_scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(active_scene.name);
            }

            if (choice_number == 1)//Exit
            {
                SceneManager.LoadScene("TitleScene");
            }
        }
    }
}
