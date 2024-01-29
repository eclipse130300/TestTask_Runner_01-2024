using System;
using CodeBase.UI.Services;
using EventBusSystem;
using Events;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private IUIService _uiService;
    
    private void Awake()
    {
        _uiService = AllServices.Container.Single<IUIService>();
    }

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
    
    [ContextMenu("SHOWWINDOW")]
    public async void SHOWWINDOW()
    {
        await _uiService.ShowPopup<PausedGameWindow>();
        
        Debug.Log("finished");
    }
    
    [ContextMenu("HIDEWINDOW")]
    public void HIDEsWINDOW()
    {
        _uiService.HidePopup<PausedGameWindow>();
    }
}