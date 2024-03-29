﻿using CodeBase.Infrastructure.CollisionDetection;
using CodeBase.Interactions;
using UnityEngine;

/// <summary>
/// Component used for enabling interactions on this gameobject
/// </summary>
public class Interactor : MonoBehaviour
{
    [SerializeField]
    private CollisionDetector _collisionDetector;

    private void OnEnable() => 
        _collisionDetector.OnTriggerEnterDetected += TriggerDetected;

    private void OnDisable() => 
        _collisionDetector.OnTriggerEnterDetected -= TriggerDetected;

    private void TriggerDetected(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            interactable.Interact(gameObject);
        }
    }
}