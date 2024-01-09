using PlayerSystem;
using UnityEngine;
using UnityEngine.AI;

namespace EnemySystem
{
    public class Enemy : MonoBehaviour
    {
        public NavMeshAgent Agent;
        public Animator Animator;
        public HumanoidMovement Movement;
        public ViewArea ViewArea;
    }
}