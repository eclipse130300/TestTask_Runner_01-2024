using CodeBase.StaticData;

public interface IStaticDataService : IService
{
    void Load();
    GameStaticData ForGame();
}