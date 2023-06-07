using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Interactions
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Interaction")]
    public class Interaction : ScriptableObject
    {
        [field:SerializeField]
        [field:SerializeReference]
        public List<InteractionElement> Elements { get; private set; } = new();
    }
}