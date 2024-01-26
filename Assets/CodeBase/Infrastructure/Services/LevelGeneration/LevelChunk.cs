using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData;
using UnityEngine;

public class LevelChunk
{
    public GameObject ViewGameObject { get; }
    public ChunkSamplePoint[,] Points { get; }
    public int MaxRows { get; }
    
    public float ChunkUnitySizeZ { get; }

    public List<LevelChunkPointsCollection> Obstacles { get; private set; } = new();

    private LevelStaticData _config;

    public LevelChunk(LevelStaticData config, GameObject chunkGameObject)
    {
        _config = config;
        ViewGameObject = chunkGameObject;
        MaxRows = config.ChunkRows;
        Points = new ChunkSamplePoint[3, MaxRows];
        ChunkUnitySizeZ = MaxRows * config.LinesSpacingZ;
        for (int y = 0; y < MaxRows; y++)
        {
            //we have 3 lines only?
            for (int x = 0; x < 3; x++)
            {
                var id = new Vector2Int(x, y);
                //-1 to 1
                var localPos = new Vector3((x - 1) * config.LinesSpacingX, 0, y * config.LinesSpacingZ);
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

            //next up
            if (rowId + 1 < MaxRows)
            {
                var adjacentUp = Points[xPos, rowId + 1];
                if(adjacentUp == null)
                    continue;
                samplePoint.AdjacentChunks.Add(adjacentUp);
            }
            
            //next down
            if (rowId - 1 >= 0)
            {
                var adjacentDown = Points[xPos, rowId - 1];
                if(adjacentDown == null)
                    continue;
                samplePoint.AdjacentChunks.Add(adjacentDown);
            }
        }
    }

    /// <summary>
    /// Use BFS search for detecting side obstacles
    /// </summary>
    public void InitializeObstaclesData()
    {
        var sideZones = GetAllSideZonesOnPath();
        var halfAmount = sideZones.Count / 2;
        //lets take the most biggest percent > 2blocks
        Obstacles = sideZones.Where(x => x.PointsCollection.Count > 2)
                             .OrderByDescending(x => x.PointsCollection.Count)
                             .Take(halfAmount)
                             .ToList();

        RemoveObstaclesAroundPathPoints();
    }

    //just making game more casual to play, adding more room for player
    private void RemoveObstaclesAroundPathPoints()
    {
        foreach (var obstacleCollection in Obstacles)
        {
            for (int i = obstacleCollection.PointsCollection.Count - 1; i >= 0; i--)
            {
                var point = obstacleCollection.PointsCollection[i];

                // Flags to track the presence of path points in different directions
                bool hasLeftOrRightPathPoint = false;
                bool hasBottomOrTopPathPoint = false;

                var startRowsToSkipInChunk = _config.DontSpawnObstaclesOnStartRowsAmount;
                bool obstaclePointOnStartRows = point.Id.y <= startRowsToSkipInChunk;

                // Check each adjacent point to determine if it's a path point in specific directions
                foreach (var adjacentPoint in point.AdjacentChunks)
                {
                    bool isPathPoint = adjacentPoint.SamplePointType == SamplePointType.Path;
                    bool isLeftOrRight = adjacentPoint.Id.x == point.Id.x - 1 || adjacentPoint.Id.x == point.Id.x + 1;
                    bool isBottomorTopPoint = adjacentPoint.Id.y == point.Id.y - 1 || adjacentPoint.Id.y == point.Id.y + 1;

                    // Update flags based on the presence of path points in different directions
                    hasLeftOrRightPathPoint |= isLeftOrRight && isPathPoint;
                    hasBottomOrTopPathPoint |= isBottomorTopPoint && isPathPoint;
                }

                // Remove the current point if there are path points both to the left/right and at the bottom
                if (hasLeftOrRightPathPoint && hasBottomOrTopPathPoint || obstaclePointOnStartRows)
                {
                    obstacleCollection.PointsCollection.Remove(point);
                }
            }
        }
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