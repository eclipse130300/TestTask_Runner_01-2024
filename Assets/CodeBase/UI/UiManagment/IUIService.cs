using System.Threading.Tasks;

namespace CodeBase.UI.Services
{
    public interface IUIService : IService
    {
        void CreateUIRoot();
        Task<bool> ShowPopup<T>() where T : BaseWindow;
        Task<bool> HidePopup<T>() where T : BaseWindow;
    }
}