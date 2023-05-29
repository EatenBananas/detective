namespace Interactions.Elements
{
    public class SetState : InteractionElement
    {
        private State _stateMachine;
        private int _state;

        public SetState(InteractionElementData data)
        {
            _stateMachine = data.StateMachine;
            _state = data.Number1;
        }
        
        public override void Execute()
        {
            _stateMachine.CurrentState = _state;
            InteractionManager.Instance.CompleteElement();
        }
    }
}
