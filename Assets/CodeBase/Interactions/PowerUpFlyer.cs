using System.Collections;
using CodeBase.Infrastructure;
using UnityEngine;

public class PowerUpFlyer : MonoBehaviour, IInteractable
{
    private IStaticDataService _staticDataService;

    private ICoroutineRunnerService _coroutineRunnerService;

    private void Awake()
    {
        _staticDataService = AllServices.Container.Single<IStaticDataService>();
        _coroutineRunnerService = AllServices.Container.Single<ICoroutineRunnerService>();
    }

    public void Interact(GameObject interactionInvoker)
    {
        if (interactionInvoker.TryGetComponent<IFlyer>(out var flyer))
        {
            _coroutineRunnerService.StartCoroutine(FlyRoutine(flyer));
        }
    }

    private IEnumerator FlyRoutine(IFlyer flyer)
    {
        flyer.StartFlying();

        yield return new WaitForSeconds(_staticDataService.ForLevel().PowerUpDuration);
        
        flyer.StopFlying();
    }
}