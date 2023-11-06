using System;
using SceneObjects;
using UnityEngine;

namespace GraphEditor.Saves
{
    [Serializable]
    public class TeleportNodeSave : GraphEditorNodeSave
    {
        [field:SerializeReference] public SceneReference Location { get; set; }
    }
}