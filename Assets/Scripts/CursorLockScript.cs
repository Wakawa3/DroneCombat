using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLockScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Screen.SetResolution(1024, 576, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.V))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (Input.GetKey(KeyCode.B)) Cursor.lockState = CursorLockMode.Confined;
        if (Input.GetKey(KeyCode.N)) Cursor.lockState = CursorLockMode.None;


        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
                Application.Quit();
#endif
        }
    }
}
