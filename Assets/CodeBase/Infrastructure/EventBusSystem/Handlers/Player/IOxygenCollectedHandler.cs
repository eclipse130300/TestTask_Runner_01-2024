namespace EventBusSystem.Handlers
{
    public interface IOxygenCollectedHandler : IGlobalSubscriber
    {
        void OnOxygenCollected();
    }
}