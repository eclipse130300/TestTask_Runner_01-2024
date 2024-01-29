using UnityEngine;
public class SpeedDownModifierProvider : MonoBehaviour, ISpeedModifierProvider
{
    private IStaticDataService _staticDataService;
    private void Awake() => 
        _staticDataService = AllServices.Container.Single<IStaticDataService>();

    public float GetSpeedModifier() => 
        _staticDataService.ForGame().SpeedUpModifier;
}