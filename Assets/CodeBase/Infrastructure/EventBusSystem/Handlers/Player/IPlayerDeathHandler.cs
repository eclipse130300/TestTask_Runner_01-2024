namespace EventBusSystem.Handlers
{
    public interface IPlayerDeathHandler : IGlobalSubscriber
    {
        void OnPlayerDeath();
    }
}