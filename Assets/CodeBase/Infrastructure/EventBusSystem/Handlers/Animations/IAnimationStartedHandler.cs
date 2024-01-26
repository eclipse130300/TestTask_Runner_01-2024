namespace EventBusSystem.Handlers
{
    public interface IAnimationStartedHandler : IGlobalSubscriber
    {
        void OnAnimationStarted();
    }
}