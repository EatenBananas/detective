using SceneObjects;
using UnityEngine;

namespace Interactions.Elements
{
    public class Cutscene : InteractionElement
    {

        [field: SerializeField] public SceneReference Clip { get; set; }
        
        public override void Execute()
        {
            SceneObjectManager.Instance.PlayCutscene(Clip);
        }

#if UNITY_EDITOR
        public override int Height() => 1;
#endif
    }
}