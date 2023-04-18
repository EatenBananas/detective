using System;
using Interactions.Elements;
using UnityEngine;

namespace Interactions
{
    public abstract class InteractionElement
    {
        public abstract void Execute();

        public static InteractionElement ToElement(InteractionElementData data)
        {
            return data.Type switch
            {
                InteractionElementType.CHANGE_CAMERA => new CameraChange(data),
                InteractionElementType.GET_KEY => new GetKey(data),
                InteractionElementType.DIALOGUE => new Dialogue(data),
                InteractionElementType.CONDITION => new Condition(data),
                InteractionElementType.SET_STATE => new SetState(data),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
