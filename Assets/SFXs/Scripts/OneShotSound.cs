using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class OneShotSound : MonoBehaviour
{
    public static OneShotSound instace { get; private set; }


    private void Awake()
    {
        if(instace != null)
        {
            Debug.LogError("Found more than one 1ShotSound in the scene");
        }
        instace = this;
    }
    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }
}
