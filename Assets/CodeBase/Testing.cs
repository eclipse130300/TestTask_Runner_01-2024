using EventBusSystem;
using Events;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [ContextMenu("START")]
    public void StartGame()
    {
        EventBus.RaiseEvent<IGameLoopStartedHandler>(h => h.OnGameLoopStated());
    }
    
    [ContextMenu("FINISH")]
    public void FinishGame()
    {
        EventBus.RaiseEvent<IGameLoopFinishedHandler>(h => h.OnGameLoopFinished());
    }
}