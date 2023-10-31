using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MenuClass
{ 
    GameObject player;
    PlayerController pc;

    [SerializeField] GameObject option_prefab = null;

    new void Start()
    {
        base.Start();

        player = GameObjectManagement.player;
        pc = player.GetComponent<PlayerController>();
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
            if (choice_number == 0)//Continue
            {
                pc.DestroyPause();
            }

            if (choice_number == 1)//Option
            {
                activated = false;
                Instantiate(option_prefab, this.transform);
            }

            if (choice_number == 2)//Restart Mission
            {
                Scene active_scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(active_scene.name);
            }

            if (choice_number == 3)//Exit
            {
                SceneManager.LoadScene("TitleScene");
            }
        }

        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Backspace))
        {
            pc.DestroyPause();
        }
    }
}
