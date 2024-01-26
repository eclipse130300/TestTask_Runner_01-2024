using CodeBase.Infrastructure.CollisionDetection;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField]
    private CollisionDetector _collisionDetector;

    private void Awake() => 
        _collisionDetector.OnTriggerEnterDetected += TriggerDetected;

    private void OnDestroy() => 
        _collisionDetector.OnTriggerEnterDetected -= TriggerDetected;

    private void TriggerDetected(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            interactable.Interact(gameObject);
        }
    }
}