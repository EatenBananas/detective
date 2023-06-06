using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Interactions.Elements
{
    [Serializable]
    public class Choice : InteractionElement
    {
        [field:SerializeField] public List<Option> Options { get; set; }
        
        public override void Execute()
        {
            UIManager.Instance.ShowOptions(
                Options.Select(opt=>opt.Text).ToList());

            InteractionManager.Instance.ListenForOptions(Options);
        }
    }
}
