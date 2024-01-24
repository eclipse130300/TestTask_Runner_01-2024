using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData;
using UnityEngine;

public class LevelChunk
{
    public GameObject ChunkGameObject { get; }
    public ChunkSamplePoint[,] Points { get; }
    public int MaxRows { get; }
    
    public float ChunkXSize { get; }

    public List<LevelChunkPointsCollection> Obstacles { get; private set; } = new();

    private LevelStaticData _config;

    public LevelChunk(LevelStaticData config, GameObject chunkGameObject, int maxRows)
    {
        _config = config;
        ChunkGameObject = chunkGameObject;
        MaxRows = maxRows;
        Points = new ChunkSamplePoint[3, maxRows];
        for (int y = 0; y < maxRows; y++)
        {
            //we have 3 lines only?
            for (int x = 0; x < 3; x++)
            {
                var id = new Vector2Int(x, y);
                //-1 to 1
                var localPos = new Vector3((x - 1) * config.SpacingBetweenPaths, 0, y);
                var samplePoint = new ChunkSamplePoint(id, localPos);

                Points[x, y] = samplePoint;
            }
        }
    }

    public void DeleteRowData(int rowId)
    {
        for (int x = 0; x < 3; x++)
        {
            Points[x, rowId] = null;
        }
    }

    public void FillRowPathData(float pathValue, int rowId)
    {
        //remap to Id version. We have only 3 columns
        var pathXPos = pathValue.AsInteger02Index();

        foreach (var samplePoint in GetAllChunkSamplePointsInfo().Where(point => point.Id.y == rowId))
        {
            if (samplePoint.Id.x == pathXPos)
                samplePoint.SamplePointType = SamplePointType.Path;

            var xPos = samplePoint.Id.x;

            //next left
            if (xPos - 1 >= 0)
            {
                var adjacentLeft = Points[xPos - 1, rowId];
                if(adjacentLeft == null)
                    continue;

                samplePoint.AdjacentChunks.Add(adjacentLeft);
            }

            //next right
            if (xPos + 1 < 3)
            {
                var adjacentRight = Points[xPos + 1, rowId];
                if(adjacentRight == null)
                    continue;
                samplePoint.AdjacentChunks.Add(adjacentRight);
            }

            //next up (no need find next down, we start indexing from down)
            if (rowId + 1 < MaxRows)
            {
                var adjacentUp = Points[xPos, rowId + 1];
                if(adjacentUp == null)
                    continue;
                samplePoint.AdjacentChunks.Add(adjacentUp);
            }
        }
    }

    /// <summary>
    /// Use BFS search for detecting side obstacles
    /// </summary>
    public void InitializeObstacles()
    {
        var sideZones = GetAllSideZonesOnPath();
        var halfAmount = sideZones.Count / 2;
        //lets take the most biggest percent > 2blocks
        Obstacles = sideZones.Where(x => x.PointsCollection.Count > 2)
                             .OrderByDescending(x => x.PointsCollection.Count)
                             .Take(halfAmount)
                             .ToList();
    }

    private HashSet<LevelChunkPointsCollection> GetAllSideZonesOnPath()
    {
        HashSet<LevelChunkPointsCollection> result = new HashSet<LevelChunkPointsCollection>();
        foreach (var chunkPoint in GetAllChunkSamplePointsInfo())
        {
            if (chunkPoint.SamplePointType == SamplePointType.None)
            {
                var obstaclePoints = FloodBFSPoints(chunkPoint);
                result.Add(new LevelChunkPointsCollection(obstaclePoints));
            }
        }

        return result;
    }

    private static List<ChunkSamplePoint> FloodBFSPoints(ChunkSamplePoint start)
    {
        List<ChunkSamplePoint> visited = new List<ChunkSamplePoint>();
        Queue<ChunkSamplePoint> work = new Queue<ChunkSamplePoint>();

        visited.Add(start);
        work.Enqueue(start);

        while (work.Count > 0)
        {
            ChunkSamplePoint current = work.Dequeue();
            foreach (var neighbour in current.AdjacentChunks)
            {
                if (!visited.Contains(neighbour) && neighbour.SamplePointType == SamplePointType.None)
                {
                    neighbour.SamplePointType = SamplePointType.Obstacle;
                    visited.Add(neighbour);
                    work.Enqueue(neighbour);
                }
            }
        }

        return visited;
    }


    private IEnumerable<ChunkSamplePoint> GetAllChunkSamplePointsInfo()
    {
        for (int y = 0; y < MaxRows; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                if(Points[x,y] != null)
                    yield return Points[x, y];
            }
        }
    }
}