using EventBusSystem;

namespace Events
{
    public interface IGameplayFinishedHandler : IGlobalSubscriber
    {
        void OnGameLoopFinished();
    }
}