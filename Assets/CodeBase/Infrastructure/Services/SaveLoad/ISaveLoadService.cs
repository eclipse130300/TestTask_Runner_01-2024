namespace CodeBase.Infrastructure.States
{
    public interface ISaveLoadService : IService
    {
        PlayerProgress LoadProgress();
        void SaveProgress();
    }
}