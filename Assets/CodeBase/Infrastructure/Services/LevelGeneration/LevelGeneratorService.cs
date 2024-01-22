using System.Collections.Generic;
using CodeBase;
using CodeBase.Infrastructure.Factory;
using UnityEngine;

public class LevelGeneratorService : ILevelGeneratorService
{
    public class LevelChunkSettings
    {
        public GameObject ChunkGameobject;
        public Vector3 StartPosition;
        public Vector3 EndPosition;
        public Vector3 ChunkLength;
    }

    private IStaticDataService _staticDataService;
    private IGameFactory _gameFactory;
    
    private Queue<GameObject> _currentChunks;

    private Vector3 _firstChunkPosition;
    private Vector3 _lastChunkPosistion;

    public LevelGeneratorService(IStaticDataService staticDataService, IGameFactory gameFactory)
    {
        _staticDataService = staticDataService;
        _gameFactory = gameFactory;
    }

    public void PreloadLevel()
    {
        var levelData = _staticDataService.ForLevel();

        var initialPoint = GameObject.FindWithTag(Constants.INITIALPOINT_TAG);
        var initialPointForward = initialPoint.transform.forward;
        
        _firstChunkPosition = initialPoint.transform.position - levelData.PreloadedFirstChunkZOffset * initialPointForward;
        var chunkXSize = (levelData.SpacingBetweenPaths + levelData.ChunkSideBorders) * 2;
        var chunkZSize = levelData.ChunkLengthZ;

        _lastChunkPosistion = _firstChunkPosition + initialPointForward * levelData.ChunkLengthZ * (levelData.PreloadedChunks - 1);

        var currentPos = _firstChunkPosition;
        
        for (int i = 0; i < levelData.PreloadedChunks; i++)
        {
            var scale = new Vector3(chunkXSize, 1, chunkZSize);
            _gameFactory.CreateGroundChunk(currentPos, initialPoint.transform.forward, scale);

            currentPos += initialPointForward * levelData.ChunkLengthZ;
        }
    }
}