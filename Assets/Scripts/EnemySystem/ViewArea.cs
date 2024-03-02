using System;
using System.Collections.Generic;
using System.Linq;
using Interactions;
using UnityEngine;

namespace EnemySystem
{
    public class ViewArea : MonoBehaviour
    {
        public event Action<GameObject> OnVictimDetected;
        
        public List<GameObject> VictimsInArea { get; private set; } = new();
        public List<GameObject> VictimsDetected { get; private set; } = new();
        public bool CanSeeVictim => VictimsDetected.Count > 0;
        
        public float Radius => _radius;
        public float Angle => _angle;
        public Transform EnemyEye => _enemyEye;

        [SerializeField, Range(0f, 100f)] private float _radius;
        [SerializeField, Range(0f, 360f)] private float _angle;
        [SerializeField] private LayerMask _victimMask;
        [SerializeField] private LayerMask _obstructionMask;
        [SerializeField] private Transform _enemyEye;
        
        private bool _isVictimDetected;
        
        private void Update()
        {
            FindVictim();
        }

        private void FindVictim()
        {
            VictimsInArea.Clear();
            VictimsDetected.Clear();
            
            // Get victim objects
            VictimsInArea = Physics.OverlapSphere(_enemyEye.position, _radius, _victimMask)
                .ToList()
                .ConvertAll(c => c.gameObject);

            if (VictimsInArea.Count <= 0) return;
            
            foreach (var victim in VictimsInArea)
            {
                var directionToVictim = (victim.transform.position - _enemyEye.position).normalized;
                if (!(Vector3.Angle(_enemyEye.forward, directionToVictim) < _angle / 2)) continue;
                    
                var distanceToVictim = Vector3.Distance(_enemyEye.position, victim.transform.position);

                if (Physics.Raycast(_enemyEye.position, directionToVictim, distanceToVictim, _obstructionMask))
                    continue;
                
                VictimsDetected.Add(victim.gameObject);
                    
                if (_isVictimDetected) continue;
                _isVictimDetected = true;
                OnVictimDetected?.Invoke(victim.gameObject);

                Debug.Log($"Victim detected: {victim.name}");
                InteractionManager.Instance.GameOver();
            }
        }
    }
}
