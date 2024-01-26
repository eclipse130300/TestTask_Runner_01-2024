namespace EventBusSystem.Handlers
{
    public interface IPlayerStangingUpHandler : IGlobalSubscriber
    {
        void OnPlayerStandingUp();
    }
}