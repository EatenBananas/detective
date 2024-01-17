using Sirenix.OdinInspector;
using UnityEngine;

namespace QuickPlayAnimationSystem
{
    public class QuickPlayAnimationManager : MonoBehaviour
    {
        [Button]
        public static void Play(Animator animator, string animationName) => animator.Play(animationName);
    }
}
