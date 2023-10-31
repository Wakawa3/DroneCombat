using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleClass4 : MonoBehaviour
{
    [SerializeField] GameObject a = null;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Destroy(a);
            a.SetActive(false);
            Debug.Log("a");
        }

        if (a == null)
        {
            Debug.Log("c");
        }
        else
        {
            Debug.Log("b");
        }
        
    }

    
}
