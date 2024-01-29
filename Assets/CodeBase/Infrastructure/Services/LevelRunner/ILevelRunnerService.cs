public interface ILevelRunnerService : IService
{
    void ModifyCurrentSpeed(float delta);
    void Run();
    void Stop();
}