namespace EventBusSystem.Handlers
{
    public interface IPlayerTookOffHandler : IGlobalSubscriber
    {
        void OnPlayerTookOff();
    }
}