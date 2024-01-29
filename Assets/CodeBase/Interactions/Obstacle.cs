using CodeBase.Hero;
using EventBusSystem;
using Events;
using UnityEngine;

namespace CodeBase.Interactions
{
    //component player can hit&die
    public class Obstacle : MonoBehaviour, IInteractable
    {
        public void Interact(GameObject interactionInvoker)
        {
            if (interactionInvoker.TryGetComponent<IDeathable>(out var deathable))
                deathable.Die();
        }
    }
}