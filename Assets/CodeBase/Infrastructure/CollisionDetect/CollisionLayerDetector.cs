using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Infrastructure.CollisionDetection
{
    public class CollisionLayerDetector
    {
        private LayerMask _detectionLayerMask;
        private bool _isActive = true;
        private bool _onlyOneDetection;

        //for preventing multiple collision enters/exits
        private List<Collider> _processingColliders = new();

        public CollisionLayerDetector(LayerMask detectionLayerMask, bool onlyOneDetection = true)
        {
            _onlyOneDetection = onlyOneDetection;
            _detectionLayerMask = detectionLayerMask;
        }

        public void DetectLayer(bool isEntering, Collider other, Action actionOnInteraction)
        {
            if(!_isActive) return;

            if (isEntering && _processingColliders.Contains(other))
                return;

            if (other.gameObject.InsideLayerMask(_detectionLayerMask))
            {
                if (isEntering)
                    _processingColliders.Add(other);

                if (!isEntering)
                    _processingColliders.Remove(other);

                actionOnInteraction?.Invoke();

                if (_onlyOneDetection)
                {
                    _isActive = false;
                }
            }
        }

        public void Activate()
        {
            _isActive = true;
        }

        public void Deactivate()
        {
            _isActive = false;
        }
    }
}