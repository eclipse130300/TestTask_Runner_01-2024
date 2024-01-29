using CodeBase.Hero;
using EventBusSystem;
using Events;
using UnityEngine;

/// <summary>
/// Component for death
/// </summary>
public class PlayerDeath : MonoBehaviour, IDeathable
{
    [SerializeField]
    private PlayerMove _playerMove;

    public void Die()
    {
        _playerMove.enabled = false;
        EventBus.RaiseEvent<IGameplayFinishedHandler>(x => x.OnGameLoopFinished());
    }
}