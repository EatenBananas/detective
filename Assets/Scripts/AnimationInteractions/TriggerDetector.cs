﻿using System;
using UnityEngine;

namespace AnimationInteractions
{
    public class TriggerDetector : MonoBehaviour
    {
        public event Action<Collider> OnEnter;
        public event Action<Collider> OnStay; 
        public event Action<Collider> OnExit;
        
        private void OnTriggerEnter(Collider other)
        {
            OnEnter?.Invoke(other);
        }
        
        private void OnTriggerStay(Collider other)
        {
            OnStay?.Invoke(other);
        }
        
        private void OnTriggerExit(Collider other)
        {
            OnExit?.Invoke(other);
        }
    }
}