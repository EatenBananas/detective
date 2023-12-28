using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundT1 : MonoBehaviour
{
    public void PlayWalkSound()
    {
        OneShotSound.instace.PlayOneShot(FMODEventsOne.instance.walkSound, transform.position);
    }
}
