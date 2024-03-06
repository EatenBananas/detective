using System.Collections;
using System.Collections.Generic;
using Interactions;
using UnityEngine;

public class PlayerAnimReceiver : MonoBehaviour
{
    public void CompleteAnim()
    {
        InteractionManager.Instance.CompleteElement();
    }
}
