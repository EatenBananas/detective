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
        [field:SerializeField] public List<InteractionElementData> Elements { get; private set; } = new();
    }
}