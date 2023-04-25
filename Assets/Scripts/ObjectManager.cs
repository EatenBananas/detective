using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Instance { get; private set; }

    
    [field:SerializeField] public List<GameObject> Objects { get; private set; } = new List<GameObject>();
    
    private void Awake()
    {
        Instance = this;
    }
    
    public string[] GetObjectNames() => Objects.Select(o => o.name).ToArray();
    public int[] GetObjectIndexes() => Objects.Select((o, i) => i).ToArray();
}
