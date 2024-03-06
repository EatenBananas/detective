using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State Machine")]
[Serializable]
public class State : ScriptableObject
{
    [field:SerializeField] 
    public List<string> States { get; private set; } = new() {"No", "Yes"};
    
    [field:SerializeField]
    [field:HideInInspector]
    public int InitialState { get; set; } = 0;

    [field:SerializeField]
    [field:HideInInspector]
    public int CurrentState { get; set; }

    public void ResetState()
    {
        CurrentState = InitialState;
    }
}
