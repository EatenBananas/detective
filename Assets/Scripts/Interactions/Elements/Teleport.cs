using System;
using UnityEngine;

namespace Interactions.Elements
{
    [Serializable]
    public class Teleport : InteractionElement
    {
        [field:SerializeField] public int TeleportId { get; set; }

        public override void Execute()
        {
            if (TeleportId > ObjectManager.Instance.Objects.Count)
            {
                Debug.LogError("Invalid teleport ID");
                return;
            }

            Vector3 destination = ObjectManager.Instance.Objects[TeleportId].transform.position;
            //PlayerTeleport.Instance.Teleport(destination);
            InteractionManager.Instance.ListenForKey(KeyCode.Escape);
        }
        
#if UNITY_EDITOR
        public override int Height() => 3;
#endif
    }
}
