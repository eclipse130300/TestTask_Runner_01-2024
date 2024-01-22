using System.Collections;
using CodeBase.Infrastructure;
using CodeBase.Services.Input;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Hero
{
    public class HeroMove : MonoBehaviour
    {
        [SerializeField]
        private CharacterController _characterController;

        [SerializeField]
        private float movementSpeed;

        private IInputService _inputService;
        private ICoroutineRunnerService _coroutineRunnerService;
        private Camera _camera;
        private bool _isStrafing;

        private void Awake()
        {
            _inputService = AllServices.Container.Single<IInputService>();
            _coroutineRunnerService = AllServices.Container.Single<ICoroutineRunnerService>();
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            var inputSigned = new Vector2(Mathf.Sign(_inputService.Axis.x), 0);
            
            Vector3 movementVector = Vector3.zero;

            if (HasInput() && !_isStrafing)
            {
                _coroutineRunnerService.StartCoroutine(DoStrafe(inputSigned));
            }
        }

        private IEnumerator DoStrafe(Vector2 input)
        {
            _isStrafing = true;

            var side = _camera.transform.TransformDirection(input);
            
            var startPos = transform.position;
            var destinationPoint = transform.position + side;

            float t = 0;
            float animationTime = 1f;
            
            while (_isStrafing)
            {
                t += Time.deltaTime;
                var newPos = Vector3.Lerp(startPos, destinationPoint, t / animationTime);
                transform.position = newPos;

                if (newPos == destinationPoint)
                {
                    _isStrafing = false;
                }
                
                yield return null;
            }
        }

        private bool HasInput()
        {
            return Mathf.Abs(_inputService.Axis.x) > Constants.Epsilon;
        }
    }
}
