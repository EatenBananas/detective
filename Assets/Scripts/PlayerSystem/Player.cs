using Sirenix.OdinInspector;
using UnityEngine;

namespace PlayerSystem
{
    [SelectionBase]
    public class Player : MonoBehaviour
    {
        [field: SerializeField] public PlayerInput Input { get; private set; }
        [field: SerializeField] public HumanoidMovement Movement { get; private set; }
        
        [Button]
        public void Teleport(Vector3 position, bool resetPlayerDestination = true)
        {
            if (resetPlayerDestination)
                Movement.Agent.ResetPath();
            
            transform.position = position;
        }

        [Button]
        public void LockPlayer(bool resetPlayerDestination = true)
        {
            Input.DisableInput();
            Movement.Agent.isStopped = true;

            if (resetPlayerDestination)
                Movement.Agent.ResetPath();
        }

        [Button]
        public void UnlockPlayer()
        {
            Movement.Agent.isStopped = false;
            Input.EnableInput();
        }
    }
}
