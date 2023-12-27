using UnityEngine;

namespace PlayerSystem
{
    [SelectionBase]
    public class Player : MonoBehaviour
    {
        [field: SerializeField] public PlayerMotionMatchingMovement MotionMatchingMovement { get; private set; }
        
        public void ReleportTo(Vector3 position)
        {
            transform.position = position;
        }
    }
}
