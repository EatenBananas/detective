using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC")]
public class DialogueNpc : ScriptableObject
{
    // avoiding name conflict with object.name
    [field:SerializeField] public string NpcName { get; set; }
}
