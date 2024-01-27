using CodeBase.Hero;
using EventBusSystem;
using Events;
using UnityEngine;

public class Obstacle : MonoBehaviour, IInteractable
{
    public void Interact(GameObject interactionInvoker)
    {
        if (interactionInvoker.TryGetComponent<PlayerMove>(out _))
        {
            //this is the player
            //we just end game
            EventBus.RaiseEvent<IGameLoopFinishedHandler>(x => x.OnGameLoopFinished());
        }
    }
}