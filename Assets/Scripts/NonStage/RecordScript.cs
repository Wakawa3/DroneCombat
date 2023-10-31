using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TitleMenuScriptより後に処理
public class RecordScript : MenuClass
{
    [SerializeField] Text score_text = null;
    [SerializeField] Text time_text = null;

    int i = 0;

    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        base.Update();

        if (choice_number <= menu_choice.Length - 3)
        {
            if (PlayerPrefs.HasKey("Stage" + (choice_number + 1) + "Score"))
            {
                score_text.text = "Score:" + PlayerPrefs.GetInt("Stage" + (choice_number + 1) + "Score");
            }
            else
            {
                score_text.text = "Score:";
            }      

            if (PlayerPrefs.HasKey("Stage" + (choice_number + 1) + "Time"))
            {
                int record_time = PlayerPrefs.GetInt("Stage" + (choice_number + 1) + "Time"); //記録されるのは秒数*100の値
                int time_minutes = Mathf.FloorToInt(record_time / 6000);
                string time_minutes_string = TimeManagement.IntToTwoDigitString(time_minutes);

                int time_seconds = Mathf.FloorToInt((record_time - time_minutes * 6000) / 100);
                string time_seconds_string = TimeManagement.IntToTwoDigitString(time_seconds);

                int time_demical = record_time - time_minutes * 6000 - time_seconds * 100;
                string time_demical_string = TimeManagement.IntToTwoDigitString(time_demical);

                time_text.text = "Time:" + time_minutes_string + ":" + time_seconds_string + ":" + time_demical_string;
            }
            else
            {
                time_text.text = "Time:";
            }
        }

        if (choice_number >= menu_choice.Length - 2)//reset back
        {
            score_text.text = "Score:";
            time_text.text = "Time:";
        }

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
        {
            if (choice_number == menu_choice.Length - 2)//reset
            {
                i++;
                if (i > 5)
                {
                    PlayerPrefs.DeleteAll();
                }
            }

            if (choice_number == menu_choice.Length - 1)//back
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
