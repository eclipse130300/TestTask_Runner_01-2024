using System;
using UnityEngine;

namespace CodeBase.Infrastructure.CollisionDetection
{
    /// <summary>
    /// Class used to avoid event trigger functions in code
    /// </summary>
    public class CollisionDetector : CollisionLayerDetectorBase
    {
        public event Action <Collider>OnCollisionEnterDetected;
        public event Action<Collider> OnTriggerEnterDetected;
        public event Action <Collider> OnCollisionExitDetected;
        public event Action<Collider> OnTriggerExitDetected;

        private void OnTriggerEnter(Collider other)
        {
            TriggerDetector.DetectLayer(true,other, () =>
            {
                OnTriggerEnterDetected?.Invoke(other);
            });
        }

        private void OnCollisionEnter(Collision other)
        {
            CollisionDetect.DetectLayer(true,other.collider, () =>
            {
                OnCollisionEnterDetected?.Invoke(other.collider);
            });
        }

        private void OnTriggerExit(Collider other)
        {
            TriggerDetector.DetectLayer(false, other, () =>
            {
                OnTriggerExitDetected?.Invoke(other);
            });
        }

        private void OnCollisionExit(Collision other)
        {
            CollisionDetect.DetectLayer(false, other.collider, () =>
            {
                OnCollisionExitDetected?.Invoke(other.collider);
            });
        }
    }
}