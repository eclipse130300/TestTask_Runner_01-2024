using UnityEngine;
public class SpeedUpModifierProvider : MonoBehaviour, ISpeedModifierProvider
{
    private IStaticDataService _staticDataService;
    private void Awake() => 
        _staticDataService = AllServices.Container.Single<IStaticDataService>();

    public float GetSpeedModifier() => 
        _staticDataService.ForLevel().SpeedUpModifier;
}