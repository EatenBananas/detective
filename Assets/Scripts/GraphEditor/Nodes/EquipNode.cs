using Equipment;
using GraphEditor.Saves;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class EquipNode : GraphEditorNode
    {
        private ObjectField _itemField;
        public EquipNode(Vector2 position) : base(position) {}
        
        protected override VisualElement GetDataContainer()
        {
            VisualElement result = new();

            _itemField = new ObjectField()
            {
                allowSceneObjects = false,
                objectType = typeof(Item),
                label = "Item"
            };
            
            result.Add(_itemField);
            
            return result;
        }

        public override GraphEditorNodeSave ToSave()
        {
            EquipNodeSave save = new();
            FillBasicProperties(save);
            
            save.Item = _itemField.value as Item;

            return save;
        }
    }
}