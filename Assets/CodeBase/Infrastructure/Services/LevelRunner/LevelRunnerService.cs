using UnityEngine;

namespace CodeBase.Services
{
    /// <summary>
    /// Service that moves level in our app (as we have infinite runner, we move not the player but the level)
    /// </summary>
    public class LevelRunnerService : ILevelRunnerService, ITickable
    {
        private readonly ILevelGeneratorService _levelGeneratorService;

        private bool _isRunning;
        private float _currentSpeed;

        public LevelRunnerService(ILevelGeneratorService levelGeneratorService, IStaticDataService staticDataService)
        {
            _levelGeneratorService = levelGeneratorService;
            _currentSpeed = staticDataService.ForGame().ScrollSpeed;
        }

        public void Run() => _isRunning = true;

        public void Stop() => _isRunning = false;

        public void Tick(float deltaTime)
        {
            if (!_isRunning)
                return;

            foreach (var generatedChunk in _levelGeneratorService.GeneratedChunks)
            {
                generatedChunk.ViewGameObject.transform.position += Vector3.back * _currentSpeed * deltaTime;
            }
        }

        public void ModifyCurrentSpeed(float delta)
        {
            _currentSpeed += delta;
        }
    }
}