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

        private IInputService _inputService;
        private ICoroutineRunnerService _coroutineRunnerService;
        private IStaticDataService _staticDataService;
        private Camera _camera;
        private bool _isStrafing;

        private Vector3 _initialPoint;

        private Coroutine _strafingCoroutine = null;

        private void Awake()
        {
            _inputService = AllServices.Container.Single<IInputService>();
            _coroutineRunnerService = AllServices.Container.Single<ICoroutineRunnerService>();
            _staticDataService = AllServices.Container.Single<IStaticDataService>();
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            var inputSigned = new Vector2(Mathf.Sign(_inputService.Axis.x), 0);
            
            var destinationPoint = CalculateDestinationPoint(inputSigned);
            var strafeTime = _staticDataService.ForLevel().StrafeAnimationTime;

            if (HasInput() && CanStrafeTo(destinationPoint) && !_isStrafing)
            {
                _isStrafing = true;
                _coroutineRunnerService.StartCoroutine(DoStrafe(destinationPoint, strafeTime));
            }
        }

        private Vector3 CalculateDestinationPoint(Vector2 inputSigned)
        {
            var side = _camera.transform.TransformDirection(inputSigned);
            var levelData = _staticDataService.ForLevel();

            var destinationPoint = transform.position + side * levelData.LinesSpacingX;
            return destinationPoint;
        }

        private bool CanStrafeTo(Vector3 destinationPoint)
        {
            var maxUnits = _staticDataService.ForLevel().LinesSpacingX;

            return !destinationPoint.VectorLengthIsGreaterThan(maxUnits);
        }

        private IEnumerator DoStrafe(Vector3 destinationPoint, float strafeTime)
        {
            Debug.Log("Started strafing");
            
            var startPos = transform.position;
            
            float t = 0;
            
            while (_isStrafing)
            {
                t += Time.deltaTime;
                var newPos = Vector3.Lerp(startPos, destinationPoint, t / strafeTime);

                if ((transform.position - newPos).magnitude > 1)
                {
                    Debug.Log("OOOOPS");
                }
                
                transform.position = newPos;

                yield return new WaitForEndOfFrame();
                
                if (newPos == destinationPoint)
                {
                    _isStrafing = false;
                }
            }
        }

        private bool HasInput()
        {
            return Mathf.Abs(_inputService.Axis.x) > Constants.Epsilon;
        }
    }
}
