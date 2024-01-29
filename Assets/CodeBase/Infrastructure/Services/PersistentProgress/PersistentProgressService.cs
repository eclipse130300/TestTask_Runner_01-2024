namespace CodeBase.Services
{
    /// <summary>
    /// grants access to persistent player data
    /// </summary>
    public class PersistentProgressService : IPersistentProgressService
    {
        public PlayerProgress Progress { get; set; }
    }
}