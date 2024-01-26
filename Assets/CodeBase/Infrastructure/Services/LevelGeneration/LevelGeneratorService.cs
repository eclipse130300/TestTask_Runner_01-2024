using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.StaticData;
using EventBusSystem;
using Events;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGeneratorService : ILevelGeneratorService, IChunkReadyForUnloadHandler, IDisposable
{
    public Queue<LevelChunk> GeneratedChunks => _generatedChunks;

    private readonly IStaticDataService _staticDataService;
    private readonly IGameFactory _gameFactory;

    private Queue<LevelChunk> _generatedChunks = new ();

    private Vector3 _firstChunkSequencePosition;
    private Vector3 _lastChunkSequencePosition;

    private int _currentRow;
    private float _randomSeedPerlinOffset;
    private float chunkUnitySizeZ;

    public LevelGeneratorService(IStaticDataService staticDataService, IGameFactory gameFactory)
    {
        _staticDataService = staticDataService;
        _gameFactory = gameFactory;
        
        EventBus.Subscribe(this);
    }

    public void InitializeLevel()
    {
        var levelData = _staticDataService.ForLevel();

        _randomSeedPerlinOffset = Random.Range(-100f, 100f);
        
        _firstChunkSequencePosition = Vector3.zero;
        _lastChunkSequencePosition = _firstChunkSequencePosition + Vector3.forward * levelData.ChunkRows * levelData.LinesSpacingZ * (levelData.PreloadedChunks - 1);

        var currentPos = _firstChunkSequencePosition;
        
        for (int i = 0; i < levelData.PreloadedChunks; i++)
        {
            CreateChunk(levelData, currentPos);
            currentPos += Vector3.forward * levelData.ChunkRows * levelData.LinesSpacingZ;
        }
    }
    private void CreateChunk(LevelStaticData staticData, Vector3 chunkPosition)
    {
        var maxRows = staticData.ChunkRows;
        var chunkXSize = (staticData.LinesSpacingX + staticData.ChunkSideBorders) * 2;
        var chunkZSize = staticData.LinesSpacingZ * maxRows;
        var linesSpacing = staticData.LinesSpacingZ;
        
        var scale = new Vector3(chunkXSize, 1, chunkZSize);
        var groundChunk = _gameFactory.CreateGroundChunk(chunkPosition, Vector3.forward, scale);
        
        var chunk = new LevelChunk(staticData, groundChunk);
        _generatedChunks.Enqueue(chunk);

        for (int i = 0; i < maxRows; i++)
        {
            _currentRow++;

            if (_currentRow < staticData.SafeRunZone)
            {
                chunk.DeleteRowData(i);
                continue;
            }

            var perlinScale = staticData.PerlinScale;

            var randomPathVal01 = Mathf.PerlinNoise((_currentRow + _randomSeedPerlinOffset) * perlinScale, 0);
            var discretePathVal = Redistribute(randomPathVal01);
            
            /*var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.position = groundChunk.transform.position 
                                  + new Vector3(discretePathVal * staticData.LinesSpacingX, 0, i * staticData.LinesSpacingZ);*/
            
            chunk.FillRowPathData(discretePathVal, i);
        }

        chunk.InitializeObstacles();

        foreach (var obstacle in chunk.Obstacles)
        {
            foreach (var obstaclePoint in obstacle.PointsCollection)
            {
                var test = GameObject.CreatePrimitive(PrimitiveType.Cube);
                test.transform.SetParent(groundChunk.transform);
                test.transform.localPosition = obstaclePoint.LocalPosition;
                test.transform.localScale = new Vector3(staticData.LinesSpacingX, 1, staticData.LinesSpacingZ);
            }
        }
    }

    //redistribute value for better path generation
    //final value is in -1 to 1 range for convenience
    private float Redistribute(float val01)
    {
        var rangeBias = 0.2f;
        var remapped = val01.Remap(0f, 1f, -1f, 1f);

        if (WithinUnsignedRange(remapped, rangeBias))
            return 0;

        return Mathf.Sign(remapped);
    }

    private bool WithinUnsignedRange(float val, float range) => 
        Mathf.Abs(val) <= range;

    public void OnChunkReadyForUnload(float overshootOffset)
    {
        //unload first chunk
        var firstChunk = _generatedChunks.Dequeue();
        GameObject.Destroy(firstChunk.ViewGameObject);
        
        //create new chunk
        var overshotPosition = new Vector3(_lastChunkSequencePosition.x,
            _lastChunkSequencePosition.y,
            _lastChunkSequencePosition.z + overshootOffset);
        
        CreateChunk(_staticDataService.ForLevel(), overshotPosition);
    }

    public void Dispose()
    {
        EventBus.Unsubscribe(this);
    }
}