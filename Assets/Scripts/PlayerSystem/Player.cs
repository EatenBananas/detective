using UnityEngine;

namespace PlayerSystem
{
    [SelectionBase]
    public class Player : MonoBehaviour
    {
        [field: SerializeField] public PlayerMotionMatchingMovement MotionMatchingMovement { get; private set; }
    }
}
