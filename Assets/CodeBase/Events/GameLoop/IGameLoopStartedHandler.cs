using EventBusSystem;

namespace Events
{
    public interface IGameLoopStartedHandler : IGlobalSubscriber
    {
        void OnGameLoopStated();
    }
}
