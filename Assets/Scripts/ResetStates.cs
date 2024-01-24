using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStates : MonoBehaviour
{
    void Start()
    {
        foreach (var state in Resources.LoadAll<State>("GameStates"))
        {
            state.ResetState();
        }
    }
    
}
