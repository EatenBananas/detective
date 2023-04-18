namespace Interactions.Elements
{
    public class SetState : InteractionElement
    {
        private StateMachine _stateMachine;
        private int _state;

        public SetState(InteractionElementData data)
        {
            _stateMachine = data.StateMachine;
            _state = data.Number1;
        }
        
        public override void Execute()
        {
            _stateMachine.State = _state;
            InteractionManager.Instance.CompleteElement();
        }
    }
}
