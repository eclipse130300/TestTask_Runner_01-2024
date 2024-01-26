using EventBusSystem;

namespace Events
{
    public interface IChunkReadyForUnloadHandler : IGlobalSubscriber
    {
        public void OnChunkReadyForUnload(float overshootOffset);
    }
}