﻿using System;
using EventBusSystem;
using Events;
using UnityEngine;

public class LevelRunnerService : ILevelRunnerService, ITickable, IGameLoopStartedHandler, IGameLoopFinishedHandler, IDisposable
{
    private readonly ILevelGeneratorService _levelGeneratorService;
    
    private bool _isRunning;
    private float _currentSpeed;

    public LevelRunnerService(ILevelGeneratorService levelGeneratorService, IStaticDataService staticDataService)
    {
        _levelGeneratorService = levelGeneratorService;
        _currentSpeed = staticDataService.ForLevel().ScrollSpeed;
        
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
            generatedChunk.ViewGameObject.transform.position += Vector3.back * _currentSpeed * deltaTime;
        }
    }

    public void ModifyCurrentSpeed(float delta)
    {
        //Debug.Log($"Modified current from {_currentSpeed} for {delta}");
        _currentSpeed += delta;
    }

    public void Dispose()
    {
        EventBus.Unsubscribe(this);
    }
}