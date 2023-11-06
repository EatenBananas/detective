using GraphEditor.Saves;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class DialogueNode : GraphEditorNode
    {
        private TextField _dialogueTextTextField;
        private ObjectField _dialogueNpcObjectField;
        
        public DialogueNode(string nodeName, Vector2 position) : base(nodeName, position) {}
        protected override VisualElement GetDataContainer()
        {
            VisualElement result = new VisualElement();

            _dialogueNpcObjectField = new ObjectField()
            {
                allowSceneObjects = false,
                objectType = typeof(DialogueNpc),
                label = "NPC"
            };
            
            _dialogueTextTextField = new TextField()
            {
                label = "Text",
                multiline = true
            };
            _dialogueTextTextField.AddToClassList("ge__textfield");
            //textTextField.AddToClassList("wide_text");
            
            result.Add(_dialogueNpcObjectField);
            result.Add(_dialogueTextTextField);
            
            return result;
        }

        public override GraphEditorNodeSave ToSave()
        {
            DialogueNodeSave save = new();
            FillBasicProperties(save);

            save.DialogueNpc = _dialogueNpcObjectField.value as DialogueNpc;
            save.DialogueText = _dialogueTextTextField.value;
            
            return save;
        }
    }
}