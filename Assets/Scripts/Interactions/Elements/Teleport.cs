using UnityEngine;

namespace Interactions.Elements
{
    public class Teleport : InteractionElement
    {
        private int _teleportId;
        
        public Teleport(InteractionElementData data)
        {
            _teleportId = data.Number1;
        }

        public override void Execute()
        {
            if (_teleportId > ObjectManager.Instance.Objects.Count)
            {
                Debug.LogError("Invalid teleport ID");
                return;
            }

            Vector3 destination = ObjectManager.Instance.Objects[_teleportId].transform.position;
            //PlayerTeleport.Instance.Teleport(destination);
            InteractionManager.Instance.ListenForKey(KeyCode.Escape);
        }
    }
}
