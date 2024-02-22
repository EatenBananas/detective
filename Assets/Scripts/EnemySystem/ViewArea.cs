using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EnemySystem
{
    public class ViewArea : MonoBehaviour
    {
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
        
        private void Update()
        {
            FindVictim();
        }

        private void FindVictim()
        {
            // Get victim objects
            VictimsInArea = Physics.OverlapSphere(_enemyEye.position, _radius, _victimMask)
                .ToList()
                .ConvertAll(_ => _.gameObject);

            if (VictimsInArea.Count <= 0) return;
            
            VictimsDetected.Clear();
            foreach (var victim in VictimsInArea)
            {
                var directionToVictim = (victim.transform.position - _enemyEye.position).normalized;
                if (!(Vector3.Angle(_enemyEye.forward, directionToVictim) < _angle / 2)) continue;
                    
                var distanceToVictim = Vector3.Distance(_enemyEye.position, victim.transform.position);
                if (!Physics.Raycast(_enemyEye.position, directionToVictim, distanceToVictim, _obstructionMask))
                    VictimsDetected.Add(victim.gameObject);
            }
        }
    }
}
