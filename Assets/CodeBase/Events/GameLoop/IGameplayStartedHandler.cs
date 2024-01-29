using EventBusSystem;

namespace Events
{
    public interface IGameplayStartedHandler : IGlobalSubscriber
    {
        void OnGameLoopStated();
    }
}
