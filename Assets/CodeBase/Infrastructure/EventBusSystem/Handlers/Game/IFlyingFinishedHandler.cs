namespace EventBusSystem.Handlers
{
    public interface IFlyingFinishedHandler : IGlobalSubscriber
    {
        void OnFlyingFinished(float multiplier);
    }
}