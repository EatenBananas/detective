using System;
using System.Collections.Generic;
using System.Linq;
using Interactions.Elements;
using UnityEngine;

namespace Interactions
{
    [Serializable]
    public abstract class InteractionElement
    {
        public abstract void Execute();

        [field: SerializeField]
        [field: HideInInspector]
        public bool Folded { get; set; } = false;
        
        [field:SerializeField]
        
        public InteractionElement NextElement { get; set; }
        
#region EDITOR LOGIC
#if UNITY_EDITOR
        
        private static List<Type> _subTypes;
        public static List<Type> SubTypes =>
            _subTypes ??= (AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes(), (assembly, type) => new {assembly, type})
                .Where(@t => @t.type.IsSubclassOf(typeof(InteractionElement)))
                .Select(@t => @t.type)).ToList();

        public abstract int Height();
#endif
#endregion
    }
}
