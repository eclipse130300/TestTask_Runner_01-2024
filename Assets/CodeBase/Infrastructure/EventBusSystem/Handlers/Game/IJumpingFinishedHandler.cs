namespace EventBusSystem.Handlers
{
    public interface IJumpingFinishedHandler : IGlobalSubscriber
    {
        void OnGameFinished();
    }
}