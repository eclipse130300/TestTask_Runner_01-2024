using System.Collections.Generic;
using CodeBase;
using CodeBase.Infrastructure.Factory;
using CodeBase.StaticData;
using UnityEngine;

public class LevelGeneratorService : ILevelGeneratorService
{
    private IStaticDataService _staticDataService;
    private IGameFactory _gameFactory;
    
    private Queue<LevelChunk> _currentChunks;

    private Vector3 _firstChunkPosition;
    private Vector3 _lastChunkPosistion;

    private int _currentRow = 0;
    private float _randomSeedPerlinOffset = 0f;
    private GameObject _initialPoint;

    public LevelGeneratorService(IStaticDataService staticDataService, IGameFactory gameFactory)
    {
        _staticDataService = staticDataService;
        _gameFactory = gameFactory;
    }

    public void InitializeLevel()
    {
        var levelData = _staticDataService.ForLevel();

        _initialPoint = GameObject.FindWithTag(Constants.INITIALPOINT_TAG);
        var initialPointForward = _initialPoint.transform.forward;

        _randomSeedPerlinOffset = Random.Range(-100f, 100f);
        
        _firstChunkPosition = _initialPoint.transform.position;
        _lastChunkPosistion = _firstChunkPosition + initialPointForward * levelData.ChunkLengthZ * (levelData.PreloadedChunks - 1);

        var currentPos = _firstChunkPosition;
        
        for (int i = 0; i < levelData.PreloadedChunks; i++)
        {
            CreateChunk(levelData, currentPos);
            currentPos += initialPointForward * levelData.ChunkLengthZ;
        }
    }

    private void CreateChunk(LevelStaticData staticData, Vector3 chunkPosition)
    {
        var chunkXSize = (staticData.SpacingBetweenPaths + staticData.ChunkSideBorders) * 2;
        var maxRows = staticData.ChunkLengthZ;
        
        var scale = new Vector3(chunkXSize, 1, maxRows);
        var groundChunk = _gameFactory.CreateGroundChunk(chunkPosition, _initialPoint.transform.forward, scale);
        
        var chunk = new LevelChunk(staticData, groundChunk, maxRows);

        for (int i = 0; i < maxRows; i++)
        {
            _currentRow++;

            if (_currentRow < staticData.SafeRunZone)
            {
                chunk.DeleteRowData(i);
                continue;
            }

            var perlinScale = staticData.PerlinScale;

            var randomPathVal01 = Mathf.PerlinNoise(((float)_currentRow + _randomSeedPerlinOffset) * perlinScale, 0);
            var descetePathVal = Redistribute(randomPathVal01);
            
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.position = groundChunk.transform.position 
                                  + new Vector3(descetePathVal * staticData.SpacingBetweenPaths, 0, i);
            
            chunk.FillRowData(descetePathVal, i);
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
}