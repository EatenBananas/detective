using UnityEngine;

namespace Interactions.Elements
{
    public class Dialogue : InteractionElement
    {
        private readonly DialogueNpc _dialogueNpc;
        private readonly string _dialogueText;

        // todo: this should be moved
        private static KeyCode DIALOGUE_ESCAPE_KEY = KeyCode.Escape;
        
        public Dialogue(InteractionElementData data)
        {
            _dialogueNpc = data.DialogueNpc;
            _dialogueText = data.Text2;
        }
        public override void Execute()
        {
            UIManager.Instance.ShowDialogue(_dialogueNpc, _dialogueText);
            InteractionManager.Instance.ListenForKey(DIALOGUE_ESCAPE_KEY);
        }
    }
}
