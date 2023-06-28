using System;
using SceneObjects;
using UnityEngine;

namespace Interactions.Elements
{
    [Serializable]
    public class Photo : InteractionElement
    {
        [field: SerializeField] public SceneReference Picture { get; set; }
        [field: SerializeField] public bool Visible { get; set; } = true;

        public override void Execute()
        {
            SceneObjectManager.Instance.UpdatePhoto(Picture, Visible);
            InteractionManager.Instance.CompleteElement();
        }
        public override int Height() => 4;
    }
}
