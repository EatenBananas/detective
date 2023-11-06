using System;
using SceneObjects;
using UnityEngine;

namespace GraphEditor.Saves
{
    [Serializable]
    public class PhotoNodeSave : GraphEditorNodeSave
    {
        [field:SerializeField] public SceneReference Picture { get; set; }
        [field:SerializeField] public bool Visible { get; set; }
    }
}