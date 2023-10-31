using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    [HideInInspector] public static int score = 0;
    [SerializeField] Text score_text = null;

    void Start()
    {
        
    }

    void Update()
    {
        score_text.text = "Score:" + score;
    }
}
