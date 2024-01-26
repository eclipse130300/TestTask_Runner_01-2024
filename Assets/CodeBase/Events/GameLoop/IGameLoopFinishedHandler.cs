using EventBusSystem;

namespace Events
{
    public interface IGameLoopFinishedHandler : IGlobalSubscriber
    {
        void OnGameLoopFinished();
    }
}