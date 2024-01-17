#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace GraphEditor
{
    public class GraphEditorSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private GraphEditorView _graphEditorView;

        private Vector2 _position = Vector2.zero;
        private Edge _edge;
        
        public void Initialize(GraphEditorView graphEditorView)
        {
            _graphEditorView = graphEditorView;
        }
        
        public void Initialize(GraphEditorView graphEditorView, Vector2 position, Edge edge)
        {
            Initialize(graphEditorView);
            _position = position;
            _edge = edge;
        }
        
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> entries = new();

            entries.Add(new SearchTreeGroupEntry(new GUIContent("Add Elements")));
            
            foreach (var nodeType in GraphEditorNode.SubTypes)
            {
                entries.Add(new SearchTreeEntry(new GUIContent(nodeType.Name))
                {
                    level = 1,
                    userData = nodeType
                });
            }
            
            //entries.Add( new SearchTreeGroupEntry(new GUIContent("Other"), 1));
            entries.Add(new SearchTreeEntry(new GUIContent("Group"))
            {
                level = 1,
                userData = new Group()
            });
            
            return entries;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            var position = _position == Vector2.zero ? context.screenMousePosition : _position;
            
            // todo: yeah this is ugly
            
            if (searchTreeEntry.userData is Group)
            {
                _graphEditorView.CreateGroup(position, true);
            }
            else
            {
                var node = _graphEditorView.CreateNode((Type) searchTreeEntry.userData, position, true);

                // if (_edge != null)
                // {
                //     GraphEditorNode previous = _edge.input.node as GraphEditorNode;
                //     previous.ConnectTo(node);
                // }
                
            }
            
            return true;
        }
    }
}
#endif