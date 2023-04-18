using UnityEngine;

namespace Interactions.Elements
{
    public class GetKey : InteractionElement
    {
        private readonly KeyCode _key;

        public GetKey(InteractionElementData data)
        {
            _key = data.KeyCode;
        }
        
        public override void Execute()
        {
            InteractionManager.Instance.ListenForKey(_key);
        }
    }
}