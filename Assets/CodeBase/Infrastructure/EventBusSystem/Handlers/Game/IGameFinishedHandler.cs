namespace EventBusSystem.Handlers
{
    public interface IGameFinishedHandler : IGlobalSubscriber
    {
        void OnGameFinished();
    }
}