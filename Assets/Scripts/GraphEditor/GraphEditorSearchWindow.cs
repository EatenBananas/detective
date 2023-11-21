using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace GraphEditor
{
    public class GraphEditorSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private GraphEditorView _graphEditorView;
        
        public void Initialize(GraphEditorView graphEditorView)
        {
            _graphEditorView = graphEditorView;
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
            // todo: yeah this is ugly
            
            if (searchTreeEntry.userData is Group)
            {
                _graphEditorView.CreateGroup(context.screenMousePosition, true);
            }
            else
            {
                _graphEditorView.CreateNode((Type) searchTreeEntry.userData, context.screenMousePosition, true);
            }
            
            return true;
        }
    }
}