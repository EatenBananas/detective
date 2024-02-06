#if UNITY_EDITOR
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
        private TextField _dialoguePathTextField;

        public DialogueNode(string nodeName, Vector2 position) : base(nodeName, position)
        {
            InitializeDataContainer();
        }

        public DialogueNode(DialogueNodeSave save) : this(save.NodeName, save.Position)
        {
            SetBasicProperties(save);
            _dialogueNpcObjectField.value = save.DialogueNpc;
            _dialogueTextTextField.value = save.DialogueText;
            _dialoguePathTextField.value = save.DialoguePath;
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
            
            _dialoguePathTextField = new TextField()
            {
                label = "Path"
            };
            _dialoguePathTextField.AddToClassList("ge__textfield");
            
            _dialogueTextTextField = new TextField()
            {
                label = "Text",
                multiline = true
            };
            _dialogueTextTextField.AddToClassList("ge__textfield");


            _dataContainer.Add(_dialogueNpcObjectField);
            _dataContainer.Add(_dialogueTextTextField);
            _dataContainer.Add(_dialoguePathTextField);
        }

        protected override VisualElement GetDataContainer() => _dataContainer;

        public override GraphEditorNodeSave ToSave()
        {
            DialogueNodeSave save = new();
            FillBasicProperties(save);

            save.DialogueNpc = _dialogueNpcObjectField.value as DialogueNpc;
            save.DialogueText = _dialogueTextTextField.value;
            save.DialoguePath = _dialoguePathTextField.value;
            
            return save;
        }

        public override InteractionElement ToInteraction()
        {
            var dialogue = ScriptableObject.CreateInstance<Dialogue>();
            
            dialogue.DialogueNpc = _dialogueNpcObjectField.value as DialogueNpc;
            dialogue.DialogueText = _dialogueTextTextField.value;
            dialogue.DialoguePath = _dialoguePathTextField.value;

            return dialogue;

        }
    }
}
#endif