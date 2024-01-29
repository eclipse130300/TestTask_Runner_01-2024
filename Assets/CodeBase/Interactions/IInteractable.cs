using UnityEngine;

namespace CodeBase.Interactions
{
    /// <summary>
    /// Common interface of all possible objects one can interact with
    /// </summary>
    public interface IInteractable
    {
        public void Interact(GameObject interactionInvoker);
    }
}