using System;
using UnityEngine;

namespace Interactions.Elements
{
    [Serializable]
    public class Dialogue : InteractionElement
    {
        [field:SerializeField] public DialogueNpc DialogueNpc { get; set; }
        [field:SerializeField] public  string DialogueText { get; set; }

        // todo: this should be moved
        private static KeyCode DIALOGUE_ESCAPE_KEY = KeyCode.Escape;
        
        public override void Execute()
        {
            UIManager.Instance.ShowDialogue(DialogueNpc, DialogueText);
            InteractionManager.Instance.ListenForKey(DIALOGUE_ESCAPE_KEY);
        }
    }
}
