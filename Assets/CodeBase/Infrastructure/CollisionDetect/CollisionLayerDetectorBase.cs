﻿using UnityEngine;

namespace CodeBase.Infrastructure.CollisionDetection
{
    public class CollisionLayerDetectorBase : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerToDetect;

        [SerializeField] private bool _onlyOnce = true;

        private CollisionLayerDetector _triggerDetector;
        private CollisionLayerDetector _collisionDetect;

        private bool _isActive;
        private void Awake()
        {
            _isActive = true;
        }

        public CollisionLayerDetector CollisionDetect => _collisionDetect ?? new CollisionLayerDetector(_layerToDetect, _onlyOnce);
        public CollisionLayerDetector TriggerDetector => _triggerDetector ?? new CollisionLayerDetector(_layerToDetect, _onlyOnce);

        public void OverrideLayer(LayerMask layerMask)
        {
            _layerToDetect = layerMask;
            Update();
        }

        private void Update()
        {
            _collisionDetect = new CollisionLayerDetector(_layerToDetect, _onlyOnce);
            _triggerDetector = new CollisionLayerDetector(_layerToDetect, _onlyOnce);
        }

        public void Activate()
        {
            TriggerDetector.Activate();
            CollisionDetect.Activate();

            _isActive = true;
        }

        public void Deactivate()
        {
            TriggerDetector.Deactivate();
            CollisionDetect.Deactivate();

            _isActive = false;
        }
    }
}