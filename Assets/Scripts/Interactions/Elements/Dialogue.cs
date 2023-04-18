using UnityEngine;

namespace Interactions.Elements
{
    public class Dialogue : InteractionElement
    {
        // todo: this should be moved to separate SO
        private readonly string _npcName;
        private readonly string _dialogueText;

        // todo: this should be moved
        private static KeyCode DIALOGUE_ESCAPE_KEY = KeyCode.Escape;
        
        public Dialogue(InteractionElementData data)
        {
            _npcName = data.Text1;
            _dialogueText = data.Text2;
        }
        public override void Execute()
        {
            UIManager.Instance.ShowDialogue(_npcName, _dialogueText);
            InteractionManager.Instance.ListenForKey(DIALOGUE_ESCAPE_KEY);
        }
    }
}
