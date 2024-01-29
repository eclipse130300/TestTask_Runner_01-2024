using UnityEngine;

namespace CodeBase.UI
{
    /// <summary>
    /// Base class for all windows
    /// </summary>
    public abstract class BaseWindow : MonoBehaviour
    {
        //we can update any window later, if we want with reactive fields of this class
        protected PlayerProgress Progress => _progressService.Progress;
        private IPersistentProgressService _progressService;

        public void Construct(IPersistentProgressService progressService)
        {
            _progressService = progressService;
        }
    }
}