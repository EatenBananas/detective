using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class DialogueNode : GraphEditorNode
    {
        public DialogueNode(Vector2 position) : base(position) {}
        protected override VisualElement GetDataContainer()
        {
            VisualElement result = new VisualElement();

            ObjectField objectField = new ObjectField()
            {
                allowSceneObjects = false,
                objectType = typeof(DialogueNpc),
                label = "NPC"
            };
            
            TextField textTextField = new TextField()
            {
                label = "Text",
                multiline = true
            };
            textTextField.AddToClassList("wide_text");
            
            result.Add(objectField);
            result.Add(textTextField);
            
            return result;
        }
    }
}