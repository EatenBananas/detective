namespace Interactions.Elements
{
    public class Condition : InteractionElement
    {
        private State _stateMachine;
        private int _equalTo;
        private Interaction _goTo;

        public Condition(InteractionElementData data)
        {
            _stateMachine = data.StateMachine;
            _equalTo = data.Number1;
            _goTo = data.Interaction;
        }
        
        public override void Execute()
        {
            if (_stateMachine.CurrentState == _equalTo)
            {
                InteractionManager.Instance.StartInteraction(_goTo);
            }
            else
            {
                InteractionManager.Instance.CompleteElement();
            }
        }
    }
}
