namespace CodeBase.Services
{
    //Interfaces used on dynamic objects to load and update persistent data
    public interface ISavedProgressReader
    {
        void LoadProgress(PlayerProgress playerProgress);
    }

    public interface ISavedProgressWriter : ISavedProgressReader
    {
        void UpdateProgress(PlayerProgress playerProgress);
    }
}