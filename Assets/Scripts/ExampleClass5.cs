using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleClass5 : MonoBehaviour
{
    
    void Start()
    {
        Debug.Log("b");
        Debug.Log(TimeManagement.passed_time);
        Debug.Log(Input.GetKeyDown(KeyCode.C));
    }

    
    void Update()
    {
        Debug.Log("c");
        Debug.Log(TimeManagement.passed_time);
        Debug.Log(Input.GetKeyDown(KeyCode.C));
    }
}
