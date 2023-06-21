using System;
using UnityEngine;

namespace Interactions.Elements
{
    [Serializable]
    public class SetState : InteractionElement
    {
        [field:SerializeField] public State StateMachine { get; set; }
        [field:SerializeField] public int State { get; set; }

        public override void Execute()
        {
            StateMachine.CurrentState = State;
            InteractionManager.Instance.CompleteElement();
        }
        
#if UNITY_EDITOR
        public override int Height() => 4;
#endif
    }
}
