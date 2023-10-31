using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClearSceneScript : MonoBehaviour
{
    [SerializeField] Text score_text = null;
    [SerializeField] Text score_new_record = null;
    [SerializeField] Text time_text = null;
    [SerializeField] Text time_new_record = null;

    void Start()
    {
        Color new_record_color = score_new_record.color;
        new_record_color.a = 1;

        score_text.text = "Score:" + ScoreScript.score;
        if (ScoreScript.score > PlayerPrefs.GetInt("Stage" + StageInitializer.stage_number + "Score", 0))
        {
            score_new_record.color = new_record_color;
            PlayerPrefs.SetInt("Stage" + StageInitializer.stage_number + "Score", ScoreScript.score);
        }


        int record_time = Mathf.FloorToInt(TimeManagement.passed_time * 100); //記録されるのは秒数*100の値
        int time_minutes = Mathf.FloorToInt(record_time / 6000);
        string time_minutes_string = TimeManagement.IntToTwoDigitString(time_minutes);

        int time_seconds = Mathf.FloorToInt((record_time - time_minutes * 6000) / 100);
        string time_seconds_string = TimeManagement.IntToTwoDigitString(time_seconds);

        int time_demical = record_time - time_minutes * 6000 - time_seconds * 100;
        string time_demical_string = TimeManagement.IntToTwoDigitString(time_demical);

        time_text.text = "Time:" + time_minutes_string + ":" + time_seconds_string + ":" + time_demical_string;
        if (record_time < PlayerPrefs.GetInt("Stage" + StageInitializer.stage_number + "Time", int.MaxValue))
        {
            time_new_record.color = new_record_color;
            PlayerPrefs.SetInt("Stage" + StageInitializer.stage_number + "Time", record_time);
        }

        PlayerPrefs.Save();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
        {
            TitleMenuScript.reserved_choice_number = 0;
            SceneManager.LoadScene("TitleScene");
        }
    }
}
