namespace CodeBase.Services
{
    public interface IStaticDataService : IService
    {
        void Load();
        GameStaticData ForGame();
    }
}