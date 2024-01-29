using UnityEngine;

namespace CodeBase.CameraLogic
{
    /// <summary>
    /// static camera following gameobject on Start only
    /// </summary>
    public class CameraStaticFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform _following;

        [SerializeField]
        private float _rotationAngleX;

        [SerializeField]
        private float _distance;

        [SerializeField]
        private float _offsetY;

        private Vector3 _followStartPos;

        private void LateUpdate()
        {
            var rotation = Quaternion.Euler(_rotationAngleX, 0 ,0);

            var position = rotation * new Vector3(0, 0, -_distance) + _followStartPos;

            transform.rotation = rotation;
            transform.position = position;
        }

        public void Follow(GameObject following)
        {
            _following = following.transform;
            _followStartPos = GetFollowingPointPosition();
        }

        private Vector3 GetFollowingPointPosition()
        {
            var followingPosition = _following.position;
            followingPosition.y += _offsetY;
            return followingPosition;
        }
    }
}