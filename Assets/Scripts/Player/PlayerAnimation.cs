using System;
using UnityEngine;

namespace Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private static readonly int IsWalking = Animator.StringToHash("isWalking");

        private void Start()
        {
            _animator.SetBool(IsWalking, false);
        }

        public void Walk(bool isWalking)
        {
            _animator.SetBool(IsWalking, isWalking);
        }
    }
}
