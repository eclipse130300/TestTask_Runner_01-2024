namespace EventBusSystem.Handlers
{
    public interface IGameReadyToPlayHandler : IGlobalSubscriber
    {
        //start - we can control our player
        void OnGameRadyToPlay();
    }
}