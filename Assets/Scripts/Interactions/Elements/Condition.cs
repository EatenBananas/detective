using System;
using UnityEngine;

namespace Interactions.Elements
{
    [Serializable]
    public class Condition : InteractionElement
    {
        [field:SerializeField] public State StateMachine { get; set; }
        [field:SerializeField] public int EqualTo { get; set; }
        [field:SerializeField] public InteractionElement GoTo { get; set; }
        
        public override void Execute()
        {
            if (StateMachine.CurrentState == EqualTo)
            {
                InteractionManager.Instance.StartInteraction(GoTo);
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
