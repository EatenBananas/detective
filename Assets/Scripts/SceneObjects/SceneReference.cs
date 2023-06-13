using UnityEngine;

namespace SceneObjects
{
    [CreateAssetMenu(fileName = "New Scene Reference")]
    public class SceneReference : ScriptableObject
    {
        // potential naming confusion with C# Type, not sure how to name it clearly
        [field:SerializeField] public SceneReferenceType SceneObjectType { get; set; }
    }
}
