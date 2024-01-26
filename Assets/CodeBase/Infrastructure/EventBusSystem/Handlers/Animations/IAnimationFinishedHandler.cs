namespace EventBusSystem.Handlers
{
    public interface IAnimationFinishedHandler : IGlobalSubscriber
    {
        void OnAnimationFinished();
    }
}