using System;
using System.Collections.Generic;
using System.Linq;
using GraphEditor.Nodes;
using GraphEditor.Saves;
using Interactions;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace GraphEditor.Utils
{
    public static class GraphEditorIOUtils
    {
        private static string _graphFileName;
        private static GraphEditorView _graphEditorView;

        private static List<GraphEditorNode> _nodes;
        private static List<GraphEditorGroup> _groups;

        private static Dictionary<string, GraphEditorGroup> _loadedGroups;
        private static Dictionary<string, GraphEditorNode> _loadedNodes;

        private static Dictionary<string, InteractionElement> _loadedInteractions;
        
        public static void Initialize(GraphEditorView graphEditorView, string graphFileName)
        {
            _graphEditorView = graphEditorView;
            _graphFileName = graphFileName;

            _nodes = new List<GraphEditorNode>();
            _groups = new List<GraphEditorGroup>();

            _loadedGroups = new Dictionary<string, GraphEditorGroup>();
            _loadedNodes = new Dictionary<string, GraphEditorNode>();
        }
        
        #region Save Methods
        
        public static void Save()
        {
            // setup
            
            CreateStaticFolders();
            GetElementsFromGraphView();
            
            // editor part
            
            GraphEditorSaveSO save = CreateAsset<GraphEditorSaveSO>("Assets/Editor/GraphEditor/Graphs", $"{_graphFileName}Graph");
            save.Initialize(_graphFileName);
            SaveGroups(save);
            SaveNodes(save);
            
            SaveAsset(save);
            
            // SO part
            
            CreateInteractions();
            UpdateConnections();
            SaveElements();
            
        }
        
        private static void CreateStaticFolders()
        {
            CreateFolder("Assets/Editor", "GraphEditor");   
            CreateFolder("Assets/Editor/GraphEditor", "Graphs");
            CreateFolder("Assets/Resources", "Interactions");
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

            T asset = LoadAsset<T>(path, assetName);
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
        
        #endregion
        
        #region Load Methods

        public static void Load()
        {
            GraphEditorSaveSO save = LoadAsset<GraphEditorSaveSO>("Assets/Editor/GraphEditor/Graphs", _graphFileName + "Graph");

            if (save is null)
            {
                EditorUtility.DisplayDialog(
                    "File not found",
                    "The specified file could not be found",
                    "ok");
                return;
            }
            
            // update filename?

            LoadGroups(save.Groups);
            LoadNodes(save.Nodes);
            LoadConnections();

        }
        
        private static void LoadGroups(List<GraphEditorGroupSave> saveGroups)
        {
            foreach (var saveGroup in saveGroups)
            {
                GraphEditorGroup group = _graphEditorView.CreateGroup(saveGroup.GroupName, saveGroup.Position);
                group.ID = saveGroup.ID;

                _loadedGroups[group.ID] = group;
            }
        }
        
        private static void LoadNodes(List<GraphEditorNodeSave> saveNodes)
        {
            foreach (var saveNode in saveNodes)
            {

                // temp solution to deal with zooming
                //Vector2 position = saveNode.Position * _graphEditorView.scale;

                GraphEditorNode node = _graphEditorView.LoadNode(saveNode);

                _loadedNodes[node.ID] = node;
                
                if (string.IsNullOrEmpty(node.GroupID))
                {
                    continue;
                }

                var group = _loadedGroups[node.GroupID];
                group.AddElement(node);
            }
        }

        private static void LoadConnections()
        {
            foreach (KeyValuePair<string,GraphEditorNode> entry in _loadedNodes)
            {
                var node = entry.Value;

                List<Edge> edges = node.LoadConnections();

                if (edges != null)
                {
                    foreach (var edge in edges)
                    {
                        _graphEditorView.AddElement(edge);
                    }

                    node.RefreshPorts();
                }
            }
        }

        public static GraphEditorNode GetNode(string uuid)
        {
            if (_loadedNodes == null)
                return null;

            if (!_loadedNodes.ContainsKey(uuid))
                return null;

            return _loadedNodes[uuid];
        }
        
        #endregion
        
        
        private static T LoadAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";

            return AssetDatabase.LoadAssetAtPath<T>(fullPath);
        }

        private static void CreateInteractions()
        {
            _loadedInteractions = new Dictionary<string, InteractionElement>();

            foreach (GraphEditorNode node in _nodes)
            {
                InteractionElement element = node.ToInteraction();

                if (element != null)
                {
                    _loadedInteractions[node.ID] = element;
                }
            }
        }

        private static void UpdateConnections()
        {
            foreach (var node in _nodes)
            {
                if (_loadedInteractions.TryGetValue(node.ID, out var interaction))
                {
                    node.UpdateConnections(interaction);
                }
            }
        }

        private static void SaveElements()
        {
            if (AssetDatabase.IsValidFolder($"Assets/Resources/Interactions/{_graphFileName}"))
            {
                AssetDatabase.DeleteAsset($"Assets/Resources/Interactions/{_graphFileName}");
            }
            CreateFolder("Assets/Resources/Interactions", _graphFileName);

            foreach (var node in _nodes)
            {
                if (_loadedInteractions.TryGetValue(node.ID, out var element))
                {
                    var interaction = CreateAsset<Interaction>($"Assets/Resources/Interactions/{_graphFileName}",
                        node.NodeName);
                    
                    interaction.Elements.Add(element);
                }
            }
            
        }
        
        public static InteractionElement GetElement(string uuid)
        {
            return _loadedInteractions[uuid];
        }
        
    }
}