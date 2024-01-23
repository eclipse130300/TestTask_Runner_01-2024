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

        var maxPathPoints = staticData.ChunkLengthZ;
        
        var chunk = new LevelChunk();
        InitializeChunkData(chunk, groundChunk, maxPathPoints);

        for (int i = 0; i < maxPathPoints; i++)
        {
            pathSamples++;
            
            if (pathSamples < staticData.SafeRunZone)
                continue;

            var perlinScale = staticData.PerlinScale;

            var randomPathVal01 = Mathf.PerlinNoise(((float)pathSamples + randomSeedPerlinOffset) * perlinScale, 0);
            var descetePathVal = Redistribute(randomPathVal01);
            var spacingZ = chunkZSize / maxPathPoints;

            /*var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.position = groundChunk.transform.position 
                                  + new Vector3(descetePathVal * staticData.SpacingBetweenPaths, 0, i * spacingZ);*/
            
            FillChunkPathData(chunk, descetePathVal, i, maxPathPoints);
        }
    }

    private static void InitializeChunkData(LevelChunk chunk, GameObject groundChunk, int maxRows)
    {
        chunk.ChunkGameObject = groundChunk;

        //lets set sample points amount = 1 unity unit
        chunk.Points = new ChunkSamplePoint[3, maxRows];

        //initialize chunk data
        for (int y = 0; y < maxRows; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                var samplePoint = new ChunkSamplePoint();
                samplePoint.IdX = x;
                samplePoint.IdY = y;

                chunk.Points[x, y] = samplePoint;
            }
        }
    }

    private void FillChunkPathData(LevelChunk levelChunk, float pathValue, int rowId, int maxRows)
    {
        //remap to Id version
        var pathColumnId = pathValue.Remap(-1f, 1f, 0, 2);
        var pathColumnIdInt = Mathf.RoundToInt(pathColumnId);

        Debug.Log(rowId);
        //we have 3 columns
        for (int i = 0; i < 3; i++)
        {
            var samplePoint = levelChunk.Points[i, rowId];
            
            if(i == pathColumnIdInt)
                samplePoint.IsPathPoint = true;
            
            //next left
            if (i - 1 >= 0)
            {
                var adjacentLeft = levelChunk.Points[i - 1, rowId];
                samplePoint.AdjacentChunks.Add(adjacentLeft);
            }
            //next right
            if (i + 1 < 3)
            {
                var adjacentRight = levelChunk.Points[i + 1, rowId];
                samplePoint.AdjacentChunks.Add(adjacentRight);
            }
            //next up (no need find next down, we start indexing from down)
            if (rowId + 1 < maxRows)
            {
                var adjacentUp = levelChunk.Points[i, rowId + 1];
                samplePoint.AdjacentChunks.Add(adjacentUp);
            }
        }
    }

    //redistribute value for better path generation
    //final value is in -1 to 1 range for convenience
    private float Redistribute(float val01)
    {
        var rangeBias = 0.2f;
        var remapped = val01.Remap(0f, 1f, -1f, 1f);

        if (WithinRange(remapped, rangeBias))
            return 0;

        return Mathf.Sign(remapped);
    }

    private bool WithinRange(float val, float range)
    {
        return val >= -range && val <= range;
    }
}