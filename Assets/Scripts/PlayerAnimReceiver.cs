using System.Collections;
using System.Collections.Generic;
using Interactions;
using UnityEngine;

public class PlayerAnimReceiver : MonoBehaviour
{
    [field: SerializeField] private GameObject _door;
    [field: SerializeField] private GameObject _waterCan;
    public void CompleteAnim()
    {
        InteractionManager.Instance.CompleteElement();
    }

    public void RemoveDoor()
    {
        _door.SetActive(false);
    }

    public void Dumb()
    {
        
    }

    public void WaterCan(int active)
    {
        _waterCan.SetActive(active == 1);
    }
}
