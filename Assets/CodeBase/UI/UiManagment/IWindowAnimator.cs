using System.Threading.Tasks;

public interface IWindowAnimator
{
    public Task AnimateShow();
    public Task AnimateHide();
}