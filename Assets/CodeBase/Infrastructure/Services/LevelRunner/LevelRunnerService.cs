using System;
using EventBusSystem;
using Events;
using UnityEngine;

public class LevelRunnerService : ILevelRunnerService, ITickable, IGameLoopStartedHandler, IGameLoopFinishedHandler, IDisposable
{
    private readonly ILevelGeneratorService _levelGeneratorService;
    
    private bool _isRunning;
    private float _runningSpeed;

    public LevelRunnerService(ILevelGeneratorService levelGeneratorService, IStaticDataService staticDataService)
    {
        _levelGeneratorService = levelGeneratorService;
        _runningSpeed = staticDataService.ForLevel().ScrollSpeed;
        
        EventBus.Subscribe(this);
    }
    
    public void OnGameLoopStated() => 
        _isRunning = true;

    public void OnGameLoopFinished() => 
        _isRunning = false;

    public void Tick(float deltaTime)
    {
        if(!_isRunning)
            return;

        foreach (var generatedChunk in _levelGeneratorService.GeneratedChunks)
        {
            generatedChunk.ViewGameObject.transform.position += Vector3.back * _runningSpeed * deltaTime;
        }
    }

    public void Dispose()
    {
        EventBus.Unsubscribe(this);
    }
}