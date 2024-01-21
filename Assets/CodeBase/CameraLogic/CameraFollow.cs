using UnityEngine;

namespace CodeBase.CameraLogic
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform _following;

        [SerializeField]
        private float _rotationAngleX;

        [SerializeField]
        private float _distance;

        [SerializeField]
        private float _offsetY;

        private void LateUpdate()
        {
            var rotation = Quaternion.Euler(_rotationAngleX, 0 ,0);

            var position = rotation * new Vector3(0, 0, -_distance) + GetFollowingPointPosition();

            transform.rotation = rotation;
            transform.position = position;
        }

        public void Follow(GameObject following) => _following = following.transform;

        private Vector3 GetFollowingPointPosition()
        {
            var followingPosition = _following.position;
            followingPosition.y += _offsetY;
            return followingPosition;
        }
    }
}