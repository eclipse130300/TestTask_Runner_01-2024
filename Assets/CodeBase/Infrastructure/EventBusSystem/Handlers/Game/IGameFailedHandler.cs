namespace EventBusSystem.Handlers
{
    public interface IGameFailedHandler : IGlobalSubscriber
    {
        void OnGameFailed();
    }
}