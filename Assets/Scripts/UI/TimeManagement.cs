using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManagement : MonoBehaviour
{
    public static float passed_time = 0;
    public float max_time_limit = 900;//秒数表示
    public bool stop_time = false;

    public float time_limit;

    float recorded_time = 0;

    [SerializeField] Text time_limit_text = null;

    void Update()
    {
        if (stop_time == false)
        {
            passed_time += Time.deltaTime;
            time_limit = max_time_limit - (passed_time - recorded_time);

            if (time_limit <= 0)
            {
                passed_time = recorded_time + max_time_limit;
                time_limit = 0;
                stop_time = true;
            }
        }

        int time_minutes = Mathf.FloorToInt(time_limit / 60);
        string time_minutes_string = IntToTwoDigitString(time_minutes);

        int time_seconds = Mathf.FloorToInt(time_limit - time_minutes * 60);
        string time_seconds_string = IntToTwoDigitString(time_seconds);

        int time_demical = Mathf.FloorToInt((time_limit - time_minutes * 60 - time_seconds) * 100);
        string time_demical_string = IntToTwoDigitString(time_demical);

        time_limit_text.text = "Limit:" + time_minutes_string + ":" + time_seconds_string + ":" + time_demical_string;
    }

    public static string IntToTwoDigitString(int number)
    {
        string two_digit_string = "";

        if (number < 10)
        {
            two_digit_string = "0" + number;
        }
        else
        {
            two_digit_string = "" + number;
        }

        return two_digit_string;
    }

    public void ResetTimeLimit(float max_time_limit)
    {
        stop_time = false;
        this.max_time_limit = max_time_limit;
        recorded_time = passed_time;
    }
}
