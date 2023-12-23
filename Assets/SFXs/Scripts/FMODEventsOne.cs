using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEventsOne : MonoBehaviour
{
    [field: Header("Menu")]
    [field: SerializeField] public EventReference buttonClick { get; private set; }

    [field: Header("Characters")]
    [field: SerializeField] public EventReference walkSound { get; private set; }

    public static FMODEventsOne instance { get; private set; }


    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Found more than one FMOD Events One instance in the scene");
        }
        instance = this;
    }
}
