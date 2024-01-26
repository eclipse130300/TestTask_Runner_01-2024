namespace EventBusSystem.Handlers
{
    public interface IChatDisplayerHandler : IGlobalSubscriber
    {
        void OnChatDisplay();
    }
}