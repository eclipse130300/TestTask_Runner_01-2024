using Cysharp.Threading.Tasks;

namespace CodeBase.UI
{
    /// <summary>
    /// Abstraction for flexible window animations (in case we need)
    /// </summary>
    public interface IWindowAnimator
    {
        public UniTask AnimateShow();
        public UniTask AnimateHide();
    }
}