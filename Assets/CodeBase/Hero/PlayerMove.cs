using System;
using System.Collections;
using CodeBase.Infrastructure;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Hero
{
    public class PlayerMove : MonoBehaviour
    {
        private IInputService _inputService;
        private ICoroutineRunnerService _coroutineRunnerService;
        private IStaticDataService _staticDataService;
        private Camera _camera;
        
        private bool _isStrafing;
        //should be handled in InputService, but as it is external module

        private void Awake()
        {
            _inputService = AllServices.Container.Single<IInputService>();
            _coroutineRunnerService = AllServices.Container.Single<ICoroutineRunnerService>();
            _staticDataService = AllServices.Container.Single<IStaticDataService>();
        }

        private void Start() => 
            _camera = Camera.main;

        private void Update()
        {
            var inputSigned = new Vector2(Math.Sign(_inputService.Axis.x), 0);
            
            if(inputSigned == Vector2.zero)
                return;
            
            var destinationPoint = CalculateDestinationPoint(inputSigned);
            var strafeTime = _staticDataService.ForGame().StrafeAnimationTime;

            if (CanStrafeTo(destinationPoint.x) && !_isStrafing)
            {
                _isStrafing = true;
                _coroutineRunnerService.StartCoroutine(DoStrafe(destinationPoint.x, strafeTime));
            }
        }

        private Vector3 CalculateDestinationPoint(Vector2 inputSigned)
        {
            var side = _camera.transform.TransformDirection(inputSigned);
            var levelData = _staticDataService.ForGame();

            var destinationPoint = transform.position + side * levelData.LinesSpacingX;
            return destinationPoint;
        }

        private bool CanStrafeTo(float xValue)
        {
            return Mathf.Abs(xValue) <= _staticDataService.ForGame().LinesSpacingX;
        }

        private IEnumerator DoStrafe(float targetX, float strafeTime)
        {
            var startPos = transform.position;
            
            float t = 0;
            
            while (_isStrafing)
            {
                t += Time.deltaTime;
                var currentPosition = transform.position;
                var newPosX = Mathf.Lerp(startPos.x, targetX, t / strafeTime);

                transform.position = new Vector3(newPosX, currentPosition.y, currentPosition.z);

                yield return new WaitForEndOfFrame();
                
                if (Mathf.Approximately(newPosX, targetX))
                {
                    _isStrafing = false;
                    //in case of rounding error
                    transform.position = new Vector3(targetX, currentPosition.y, currentPosition.z);
                }
            }
        }
    }
}
