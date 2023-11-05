namespace AvoidGame.Interface
{
    public interface IGameStateManager
    {
        public GameState GameState { get; }
        public void MoveToNextState();
    }
}
