using Interactions;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class ConditionNode : GraphEditorNode
    {
        public ConditionNode(Vector2 position) : base(position) {}

        protected override VisualElement GetDataContainer()
        {
            VisualElement result = new();

            ObjectField stateField = new ObjectField()
            {
                allowSceneObjects = false,
                objectType = typeof(State),
                label = "State"
            };

            // todo: rozwijać to z listy
            IntegerField equalToField = new IntegerField()
            {
                label = "Equal to"
            };

            // todo: this must be a node connection
            ObjectField interactionField = new ObjectField()
            {
                allowSceneObjects = false,
                objectType = typeof(Interaction),
                label = "Go to"
            };
            
            result.Add(stateField);
            result.Add(equalToField);
            result.Add(interactionField);
            
            return result;
        }
    }
}