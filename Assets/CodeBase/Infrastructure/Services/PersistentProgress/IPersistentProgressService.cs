namespace CodeBase.Services
{
    //abstraction for gaining persistent player data
    public interface IPersistentProgressService : IService
    {
        PlayerProgress Progress { get; set; }
    }
}