namespace CodeBase.Hero
{
    public interface ISavedProgressReader
    {
        void LoadProgress(PlayerProgress playerProgress);
    }

    public interface ISavedProgressWriter : ISavedProgressReader
    {
        void UpdateProgress(PlayerProgress playerProgress);
    }
}