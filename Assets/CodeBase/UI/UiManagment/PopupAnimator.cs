using System;
using System.Collections;
using System.Threading.Tasks;
using CodeBase.Infrastructure;
using UnityEditor;
using UnityEngine;

public class PopupAnimator : MonoBehaviour, IWindowAnimator
{
    [SerializeField]
    private CanvasGroup _canvasGroup;

    private ICoroutineRunnerService _coroutineRunnerService;
    private IStaticDataService _staticDataService;

    private float _animationTime;

    private void Awake()
    {
        _coroutineRunnerService = AllServices.Container.Single<ICoroutineRunnerService>();
        _staticDataService = AllServices.Container.Single<IStaticDataService>();

        _animationTime = _staticDataService.ForGame().UiAnimationTime;
    }

    public Task AnimateShow()
    {
        var staticData = _staticDataService.ForGame();
        
        _canvasGroup.alpha = staticData.UiMinAlpha;;
        transform.localScale = Vector3.one * staticData.UiMaxScale;
        
        var tcs = new TaskCompletionSource<bool>();

        _coroutineRunnerService.StartCoroutine(AnimateRoutine(1, 1));

        return Task.Delay(TimeSpan.FromSeconds(_animationTime));
    }

    public Task AnimateHide()
    {
        var staticData = _staticDataService.ForGame();
        
        _canvasGroup.alpha = 1;
        transform.localScale = Vector3.one;
        
        var tcs = new TaskCompletionSource<bool>();
        
        _coroutineRunnerService.StartCoroutine(AnimateRoutine(staticData.UiMinAlpha, staticData.UiMaxScale));

        return Task.Delay(TimeSpan.FromSeconds(_animationTime));;
    }

    private IEnumerator AnimateRoutine(float toAlpha, float toScale)
    {
        var t = 0f;
        var startAlpha = _canvasGroup.alpha;
        var startScale = transform.localScale.x;
        
        while (t <= _animationTime)
        {
            UpdateAnimation(t, startAlpha, startScale, toAlpha, toScale);

            yield return null;
            
            t += Time.deltaTime;
        }
        //run one more time to ensure t01 > 1
        UpdateAnimation(t, startAlpha, startScale, toAlpha, toScale);
    }
    
    private void UpdateAnimation(float t, float startAlpha, float startScale, float toAlpha, float toScale)
    {
        //we could do easing as well
        var t01 = Mathf.Clamp01(t / _animationTime);
        var alpha = Mathf.Lerp(startAlpha, toAlpha, t01);
        var scaleMultiplier = Mathf.Lerp(startScale, toScale, t01);

        _canvasGroup.alpha = alpha;
        transform.localScale = Vector3.one * scaleMultiplier;
    }
}