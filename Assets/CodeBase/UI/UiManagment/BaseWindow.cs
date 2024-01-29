using UnityEngine;

public abstract class BaseWindow : MonoBehaviour
{
    protected IPersistentProgressService _progressService;
    protected PlayerProgress Progress => _progressService.Progress;

    public virtual void Construct(IPersistentProgressService progressService)
    {
        _progressService = progressService;
    }
}