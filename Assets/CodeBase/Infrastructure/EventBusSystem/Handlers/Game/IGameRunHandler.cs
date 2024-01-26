namespace EventBusSystem.Handlers
{
    public interface IGameRunHandler : IGlobalSubscriber
    {
        //run is we just pressed tap to play
        void OnGameRun();
    }
}