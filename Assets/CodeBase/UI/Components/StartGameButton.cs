using EventBusSystem;
using Events;
using UnityEngine;
using UnityEngine.UI;

public class StartGameButton : MonoBehaviour
{
    [SerializeField]
    private Button _startButton;

    private void Awake() => 
        _startButton.onClick.AddListener(StartGame);

    private void StartGame() => 
        EventBus.RaiseEvent<IGameplayStartedHandler>(h => h.OnGameLoopStated());

    private void OnDestroy() => 
        _startButton.onClick.RemoveListener(StartGame);
}