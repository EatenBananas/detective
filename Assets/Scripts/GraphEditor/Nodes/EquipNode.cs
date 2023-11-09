using Equipment;
using GraphEditor.Saves;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphEditor.Nodes
{
    public class EquipNode : GraphEditorNode
    {
        private VisualElement _dataContainer;
        private ObjectField _itemField;

        public EquipNode(string nodeName, Vector2 position) : base(nodeName, position)
        {
            InitializeDataContainer();
        }

        public EquipNode(EquipNodeSave save) : this(save.NodeName, save.Position)
        {
            SetBasicProperties(save);
            _itemField.value = save.Item;
        }

        private void InitializeDataContainer()
        {
            _dataContainer = new VisualElement();
            
            _itemField = new ObjectField()
            {
                allowSceneObjects = false,
                objectType = typeof(Item),
                label = "Item"
            };
            
            _dataContainer.Add(_itemField);
        }

        protected override VisualElement GetDataContainer() => _dataContainer;

        public override GraphEditorNodeSave ToSave()
        {
            EquipNodeSave save = new();
            FillBasicProperties(save);
            
            save.Item = _itemField.value as Item;

            return save;
        }
    }
}