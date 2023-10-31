using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example3Class : MonoBehaviour
{
    public LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        mask = LayerMask.GetMask("Enemy", "Default");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, mask))
        {
            Debug.Log(hit.collider);
        }
    }
}
