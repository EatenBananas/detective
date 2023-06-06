using System;
using Interactions.Elements;
using UnityEngine;

namespace Interactions
{
    [Serializable]
    public abstract class InteractionElement
    {
        public abstract void Execute();
    }
}
