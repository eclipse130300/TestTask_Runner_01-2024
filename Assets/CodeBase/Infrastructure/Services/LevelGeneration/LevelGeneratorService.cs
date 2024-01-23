using System.Collections.Generic;
using CodeBase;
using CodeBase.Infrastructure.Factory;
using CodeBase.StaticData;
using UnityEngine;

public class LevelGeneratorService : ILevelGeneratorService
{ 
    public class LevelChunk
    {
        public struct ValueRange
        {
            public float Start;
            public float End;
        }

        public GameObject ChunkGameobject;
        public float ChunkLengthUnits;

        public float[] Path;
        public float[] PathDiscrete;

        public ValueRange[] StraightPath;
    }

    private IStaticDataService _staticDataService;
    private IGameFactory _gameFactory;
    
    private Queue<GameObject> _currentChunks;

    private Vector3 _firstChunkPosition;
    private Vector3 _lastChunkPosistion;

    private int pathSamples = 0;
    private float randomSeedPerlinOffset = 0f;

    public LevelGeneratorService(IStaticDataService staticDataService, IGameFactory gameFactory)
    {
        _staticDataService = staticDataService;
        _gameFactory = gameFactory;
    }

    public void InitializeLevel()
    {
        var levelData = _staticDataService.ForLevel();

        var initialPoint = GameObject.FindWithTag(Constants.INITIALPOINT_TAG);
        var initialPointForward = initialPoint.transform.forward;

        randomSeedPerlinOffset = Random.Range(-100f, 100f);
        
        _firstChunkPosition = initialPoint.transform.position;
        _lastChunkPosistion = _firstChunkPosition + initialPointForward * levelData.ChunkLengthZ * (levelData.PreloadedChunks - 1);

        var currentPos = _firstChunkPosition;
        
        for (int i = 0; i < levelData.PreloadedChunks; i++)
        {
            CreateChunk(levelData, currentPos, initialPoint);
            currentPos += initialPointForward * levelData.ChunkLengthZ;
        }
    }

    private void CreateChunk(LevelStaticData staticData, Vector3 chunkPosition, GameObject initialPoint)
    {
        var chunkXSize = (staticData.SpacingBetweenPaths + staticData.ChunkSideBorders) * 2;
        var chunkZSize = staticData.ChunkLengthZ;
        
        var scale = new Vector3(chunkXSize, 1, chunkZSize);
        var groundChunk = _gameFactory.CreateGroundChunk(chunkPosition, initialPoint.transform.forward, scale);

        //lets set sample points amount = 1 unity unit
        var maxPathPoints = staticData.ChunkLengthZ;

        for (int i = 0; i < maxPathPoints; i++)
        {
            pathSamples++;
            
            if (pathSamples < staticData.SafeRunZone)
                continue;

            var perlinScale = staticData.PerlinScale;
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var randomVal = Mathf.PerlinNoise(((float)pathSamples + randomSeedPerlinOffset) * perlinScale, 0);

            Debug.Log(randomVal);
            
            var spacing = chunkZSize / maxPathPoints;

            go.transform.position = groundChunk.transform.position + new Vector3(randomVal, 0, i * spacing);
        }

    }
}