namespace EventBusSystem.Handlers
{
    public interface IPlayerGetOxygenHandler : IGlobalSubscriber
    {
        void OnPlayerGetOxygen();
    }
}