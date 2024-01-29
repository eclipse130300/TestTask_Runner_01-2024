using Cysharp.Threading.Tasks;

namespace CodeBase.UI
{
    public interface IUIService : IService
    {
        void CreateUIRoot();
        UniTask<bool> ShowPopup<T>() where T : BaseWindow;
        UniTask<bool> HidePopup<T>() where T : BaseWindow;
        void CleanUp();
    }
}