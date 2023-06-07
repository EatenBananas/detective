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
        
#region EDITOR LOGIC
#if UNITY_EDITOR
        
        private static List<Type> _subTypes;
        public static List<Type> SubTypes =>
            _subTypes ??= (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                where type.IsSubclassOf(typeof(InteractionElement))
                select type).ToList();

        public abstract int Height();

#endif

        #endregion
    }
}
