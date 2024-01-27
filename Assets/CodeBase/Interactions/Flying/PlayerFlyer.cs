using System;
using System.Collections;
using UnityEngine;

public class PlayerFlyer : MonoBehaviour, IFlyer
{
    [SerializeField]
    private PlayerInteractor _playerInteractor;
    private bool _isFlying;

    private IStaticDataService _staticDataService;

    private void Awake()
    {
        _staticDataService = AllServices.Container.Single<IStaticDataService>();
    }

    public void StartFlying()
    {
        _playerInteractor.enabled = false;
        
        var targetY = _staticDataService.ForLevel().FlyingHeight;
        StartCoroutine(FlyingRoutine(targetY, EaseInCubic));
    }

    public void StopFlying()
    {
        _playerInteractor.enabled = true;
        
        StartCoroutine(FlyingRoutine(0, EaseOutCubic));

    }
    private IEnumerator FlyingRoutine(float targetY, Func<float,float> easing)
    {
        var startY = transform.position.y;
        float t = 0;
        var animTime = _staticDataService.ForLevel().TakeOffTime;
        
        while (true)
        {
            t += Time.deltaTime;
            var currentPosition = transform.position;

            var time01 = Mathf.Clamp01(t / animTime);
            var timeEased = easing(time01);
            
            var newPosY = Mathf.Lerp(startY, targetY, timeEased);

            transform.position = new Vector3(currentPosition.x, newPosY, currentPosition.z);

            yield return new WaitForEndOfFrame();
                
            if (Mathf.Approximately(newPosY, targetY))
            {
                //in case of rounding error
                transform.position = new Vector3(currentPosition.x, targetY, currentPosition.z);
                yield break;
            }
        }
    }

    private float EaseInCubic(float x) => 
        x * x * x;

    private float EaseOutCubic(float x) => 
        1 - Mathf.Pow(1 - x, 3);
}