using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interactions.Elements
{
    [Serializable]
    public class SceneChange : InteractionElement
    {
        [field: SerializeField] public int SceneIndex { get; set; } = 0;
        
        public override void Execute()
        {
            SceneManager.LoadScene(SceneIndex);
        }
        
#if UNITY_EDITOR
        public override int Height() => 3;
#endif
    }
}
