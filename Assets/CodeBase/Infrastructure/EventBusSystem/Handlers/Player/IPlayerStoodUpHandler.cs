namespace EventBusSystem.Handlers
{
    public interface IPlayerStoodUpHandler : IGlobalSubscriber
    {
        void OnPlayerStoodUp();
    }
}