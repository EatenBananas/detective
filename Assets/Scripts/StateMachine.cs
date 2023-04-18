using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State Machine")]
[Serializable]
public class StateMachine : ScriptableObject
{
    [field:SerializeField] 
    public List<string> States { get; private set; } = new() {"No", "Yes"};
    
    // todo: nice list
    [field:SerializeField]
    public int InitialState { get; private set; } = 0;

    public int State { get; set; }

    private void OnEnable()
    {
        State = InitialState;
    }
}
