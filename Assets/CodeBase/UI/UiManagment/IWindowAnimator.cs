using Cysharp.Threading.Tasks;

public interface IWindowAnimator
{
    public UniTask AnimateShow();
    public UniTask AnimateHide();
}