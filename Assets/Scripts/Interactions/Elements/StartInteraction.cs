using System;
using UnityEngine;

namespace Interactions.Elements
{
    [Serializable]
    public class StartInteraction : InteractionElement
    {
        [field:SerializeField] public string Title { get; set; }
        public override void Execute()
        {
            Debug.Log($"Starting interaction: {Title}");
            InteractionManager.Instance.CompleteElement();
        }

#if UNITY_EDITOR
        public override int Height() => 1;
#endif
    }
}