using UnityEngine;

namespace CodeBase.Infrastructure.CollisionDetection
{
    public class CollisionLayerDetectorBase : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerToDetect;

        [SerializeField] private bool _onlyOnce = true;

        private CollisionLayerDetector _triggerDetector;
        private CollisionLayerDetector _collisionDetect;

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
        }

        public void Deactivate()
        {
            TriggerDetector.Deactivate();
            CollisionDetect.Deactivate();
        }
    }
}