using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectGroupScript : MonoBehaviour
{
    void Awake()
    {
        transform.DetachChildren();
        Destroy(this.gameObject);
    }
}
