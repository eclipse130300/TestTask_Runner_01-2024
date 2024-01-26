using UnityEngine;

/// <summary>
/// Common interface of all possible objects player can interact with
/// </summary>
public interface IInteractable
{
    public void Interact(GameObject interactionInvoker);
}