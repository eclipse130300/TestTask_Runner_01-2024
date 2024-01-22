using CodeBase.Infrastructure;
using CodeBase.Services.Input;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Hero
{
    public class HeroMove : MonoBehaviour, ISavedProgressWriter
    {
        [SerializeField]
        private CharacterController _characterController;

        [SerializeField]
        private float movementSpeed;

        private IInputService _inputService;
        private Camera _camera;

        private void Awake()
        {
            _inputService = AllServices.Container.Single<IInputService>();
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            Vector3 movementVector = Vector3.zero;

            if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
            {
                movementVector = _camera.transform.TransformDirection(_inputService.Axis);
                movementVector.y = 0;
                movementVector.Normalize();

                transform.forward = movementVector;
            }

            movementVector += Physics.gravity;
        
            _characterController.Move(movementVector * movementSpeed * Time.deltaTime);
        }
        
        public void UpdateProgress(PlayerProgress playerProgress)
        {
            playerProgress.WorldData.PositionOnLevel =
                new PositionOnLevel(GetCurrentLevel(), transform.position.AsVector3Data());
        }

        public void LoadProgress(PlayerProgress playerProgress)
        {
            if (playerProgress.WorldData.PositionOnLevel.Level != GetCurrentLevel())
                return;

            Vector3Data savedPosition = playerProgress.WorldData.PositionOnLevel.Position;
            
            if (savedPosition == null)
                return;

            Warp(to: savedPosition);
        }

        private void Warp(Vector3Data to)
        {
            _characterController.enabled = false;
            transform.position = to.AsUnityVector().AddY(_characterController.height);
            _characterController.enabled = true;
        }

        private static string GetCurrentLevel()
        {
            return SceneManager.GetActiveScene().name;
        }
    }
}