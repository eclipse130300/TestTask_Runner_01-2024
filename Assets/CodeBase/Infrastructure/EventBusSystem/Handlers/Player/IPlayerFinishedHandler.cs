namespace EventBusSystem.Handlers
{
    public interface IPlayerFinishedHandler : IGlobalSubscriber
    {
        void OnPlayerFinished();
    }
}