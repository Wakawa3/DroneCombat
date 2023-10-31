using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example2Class : MonoBehaviour
{
    bool b = true;
    IEnumerator c = null;

    void Start()
    {
        

        

    }

    private void FixedUpdate()
    {
        

        Debug.Log("e");
        Debug.Log(Time.realtimeSinceStartup);
        if (b)
        {
            c = a();

            Debug.Log("a");
            StartCoroutine(c);
           // StopCoroutine(c);
            Debug.Log("b");
        }
        //else
       // {
        //    Debug.Log("g");
         //   StartCoroutine(c);
       //     Debug.Log("h");
       // }
        Debug.Log("f");
    }

    IEnumerator a()
    {
        b = false;

        Debug.Log("c");
        yield return new WaitForFixedUpdate();
        Debug.Log("d");
        Debug.Log(Time.realtimeSinceStartup);

        b = true;
    } 

}
