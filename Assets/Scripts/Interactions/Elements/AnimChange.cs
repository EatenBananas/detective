using SceneObjects;
using UnityEngine;

namespace Interactions.Elements
{
    public class AnimChange : InteractionElement
    {
        [field:SerializeField] public string AnimName { get; set; }
        public override void Execute()
        {
            SceneObjectManager.Instance.PlayAnim(AnimName);
            //InteractionManager.Instance.CompleteElement();
        }
        
        
#if UNITY_EDITOR
public override int Height()
{
    return 1;
}
#endif
    }
}