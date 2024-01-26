using System;
using System.Collections;
using CodeBase.Infrastructure;
using UnityEngine;

[RequireComponent(typeof(ISpeedModifierProvider))]
public class PowerUpSpeedModifierInteractable : MonoBehaviour, IInteractable
{
    private ILevelRunnerService _levelRunnerService;
    private IStaticDataService _staticDataService;

    private ISpeedModifierProvider _speedModifierProvider;
    private ICoroutineRunnerService _coroutineRunnerService;

    private void Awake()
    {
        _speedModifierProvider = GetComponent<ISpeedModifierProvider>();

        _levelRunnerService = AllServices.Container.Single<ILevelRunnerService>();
        _staticDataService = AllServices.Container.Single<IStaticDataService>();
        _coroutineRunnerService = AllServices.Container.Single<ICoroutineRunnerService>();
    }

    public void Interact(GameObject interactionInvoker) => 
        _coroutineRunnerService.StartCoroutine(PowerUpRoutine());

    private IEnumerator PowerUpRoutine()
    {
        var speedModifier = _speedModifierProvider.GetSpeedModifier();
        _levelRunnerService.ModifyCurrentSpeed(speedModifier);

        yield return new WaitForSeconds(_staticDataService.ForLevel().PowerUpDuration);
        
        _levelRunnerService.ModifyCurrentSpeed(-speedModifier);
    }
}