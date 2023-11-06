using SceneObjects;
using UnityEngine;

namespace GraphEditor.Saves
{
    public class CameraChangeNodeSave : GraphEditorNodeSave
    {
        [field:SerializeField] public SceneReference Camera { get; set; }
    }
}