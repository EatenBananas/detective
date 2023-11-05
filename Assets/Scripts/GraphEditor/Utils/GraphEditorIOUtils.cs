using System;
using System.Collections.Generic;
using System.Linq;
using GraphEditor.Save;
using UnityEditor;
using UnityEngine;

namespace GraphEditor.Utils
{
    public static class GraphEditorIOUtils
    {
        private static string _graphFileName;
        private static GraphEditorView _graphEditorView;

        private static List<GraphEditorNode> _nodes;
        private static List<GraphEditorGroup> _groups;

        public static void Initialize(GraphEditorView graphEditorView, string graphFileName)
        {
            _graphEditorView = graphEditorView;
            _graphFileName = graphFileName;

            _nodes = new List<GraphEditorNode>();
            _groups = new List<GraphEditorGroup>();
        }
        
        public static void Save()
        {
            CreateStaticFolders();
            GetElementsFromGraphView();
            GraphEditorSaveSO save = CreateAsset<GraphEditorSaveSO>("Assets/Editor/GraphEditor/Graphs", $"{_graphFileName}Graph");
            save.Initialize(_graphFileName);
            SaveGroups(save);
            SaveNodes(save);
            
            SaveAsset(save);
        }
        
        private static void CreateStaticFolders()
        {
            CreateFolder("Assets/Editor", "GraphEditor");   
            CreateFolder("Assets/Editor/GraphEditor", "Graphs");   
        }
        private static void CreateFolder(string path, string folderName)
        {
            if (AssetDatabase.IsValidFolder($"{path}/{folderName}"))
            {
                return;
            }

            AssetDatabase.CreateFolder(path, folderName);
        }
        
        private static void GetElementsFromGraphView()
        {
            _graphEditorView.graphElements.ForEach(graphElement =>
            {
                switch (graphElement)
                {
                    case GraphEditorNode node:
                        _nodes.Add(node);
                        return;
                    case GraphEditorGroup group:
                        _groups.Add(group);
                        return;
                }
            });
        }
        
        private static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";

            T asset = AssetDatabase.LoadAssetAtPath<T>(fullPath);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, fullPath);
            }
            return asset;
        }

        private static void SaveAsset(UnityEngine.Object asset)
        {
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        private static void SaveGroups(GraphEditorSaveSO save)
        {
            List<string> groupNames = new();
            foreach (var group in _groups)
            {
                save.Groups.Add(group.ToSave());
                groupNames.Add(group.title);
            }
            // UpdateOldGroups(groupNames, save);
        }

        // private static void UpdateOldGroups(List<string> currentGroupNames, GraphEditorSaveSO save)
        // {
        //     if (save.OldGroupNames != null && save.OldGroupNames.Count > 0)
        //     {
        //         List<string> groupsToRemove = save.OldGroupNames.Except(currentGroupNames).ToList();
        //         foreach (var groupToRemove in groupsToRemove)
        //         {
        //             
        //         }
        //     }
        // }
        
        private static void SaveNodes(GraphEditorSaveSO save)
        {
            foreach (var node in _nodes)
            {
                save.Nodes.Add(node.ToSave());
            }
        }
    }
}