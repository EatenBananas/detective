using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStates : MonoBehaviour
{
    void Start()
    {
       // Debug.Log(PlayerPrefs.GetInt("new", 1));

        if (PlayerPrefs.GetInt("new", 1) == 1)
        {
            Debug.Log("Resetting Scriptable Objects...");
            foreach (var state in Resources.LoadAll<State>("GameStates"))
            {
                state.ResetState();
            }
        }
    }
    
}
