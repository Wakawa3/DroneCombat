using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInitializer : MonoBehaviour
{
    public static int stage_number = 0;
    [SerializeField] int stage_number_set = 0;

    void Start()
    {
        stage_number = stage_number_set;

        Time.timeScale = 1;

        ScoreScript.score = 0;
        TimeManagement.passed_time = 0;
    }
}
