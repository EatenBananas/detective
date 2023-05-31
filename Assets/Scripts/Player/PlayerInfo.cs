using UnityEngine;

namespace Player
{
    public class PlayerInfo : MonoBehaviour
    {
        public static Transform PlayerTransform { get; private set; }

        private void Awake()
        {
            PlayerTransform = GetComponent<Transform>();
        }
    }
}
