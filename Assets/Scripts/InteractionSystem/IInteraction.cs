namespace InteractionSystem
{
    public interface IInteraction
    {
        public bool CanInteract();

        public void OnEnter();
        public void OnStay();
        public void OnDown();
        public void OnExit();
    }
}