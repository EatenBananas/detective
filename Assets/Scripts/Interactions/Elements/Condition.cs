using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Interactions.Elements
{
    [Serializable]
    public class Condition : InteractionElement
    {
        [field:SerializeField] public State StateMachine { get; set; }
        
        [field:SerializeField] public SerializableDictionary<int, InteractionElement> Outcomes { get; set; }
        
        public override void Execute()
        {
            if (Outcomes.TryGetValue(StateMachine.CurrentState, out var outcome))
            {
                InteractionManager.Instance.StartInteraction(outcome);
            }
            else
            {
                InteractionManager.Instance.CompleteElement();
            }
        }
        
#if UNITY_EDITOR
        public override int Height() => 6;
#endif
    }
}
