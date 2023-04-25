using System;
using System.Collections;
using System.Collections.Generic;
using Prototypes;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    public static PlayerTeleport Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Teleport(Vector3 position)
    {
        gameObject.transform.position = position;
        
        //temp
        
        GetComponent<PrototypeMovement>().Lock();
    }
}
