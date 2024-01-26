namespace EventBusSystem.Handlers
{
    public interface IPlayerSpawnedHandler : IGlobalSubscriber
    {
        void OnPlayerSpawned();
    }
}