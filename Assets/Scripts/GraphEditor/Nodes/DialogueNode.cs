using GraphEditor.Saves;
using Interactions;
using Interactions.Elements;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class DialogueNode : GraphEditorNode
    {
        private VisualElement _dataContainer;
        private TextField _dialogueTextTextField;
        private ObjectField _dialogueNpcObjectField;

        public DialogueNode(string nodeName, Vector2 position) : base(nodeName, position)
        {
            InitializeDataContainer();
        }

        public DialogueNode(DialogueNodeSave save) : this(save.NodeName, save.Position)
        {
            SetBasicProperties(save);
            _dialogueNpcObjectField.value = save.DialogueNpc;
            _dialogueTextTextField.value = save.DialogueText;
        }

        private void InitializeDataContainer()
        {
            _dataContainer = new VisualElement();
            
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
            
            _dataContainer.Add(_dialogueNpcObjectField);
            _dataContainer.Add(_dialogueTextTextField);
        }

        protected override VisualElement GetDataContainer() => _dataContainer;

        public override GraphEditorNodeSave ToSave()
        {
            DialogueNodeSave save = new();
            FillBasicProperties(save);

            save.DialogueNpc = _dialogueNpcObjectField.value as DialogueNpc;
            save.DialogueText = _dialogueTextTextField.value;
            
            return save;
        }

        public override InteractionElement ToInteraction()
        {
            return new Dialogue()
            {
                DialogueNpc = _dialogueNpcObjectField.value as DialogueNpc,
                DialogueText = _dialogueTextTextField.value
            };
        }
    }
}