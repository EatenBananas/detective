using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Interactions.Elements
{
    public class Choice : InteractionElement
    {
        private readonly List<Option> _options;
    
        public Choice(InteractionElementData data)
        {
            _options = data.Options;
        }

        public override void Execute()
        {
            UIManager.Instance.ShowOptions(
                _options.Select(opt=>opt.Text).ToList());

            InteractionManager.Instance.ListenForOptions(_options);
        }
    }
}
