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

    public LevelChunk(LevelStaticData config, GameObject chunkGameObject, int maxRows)
    {
        ChunkGameObject = chunkGameObject;
        MaxRows = maxRows;

        //we have 3 lines only?
        Points = new ChunkSamplePoint[3, maxRows];

        //initialize chunk data
        for (int y = 0; y < maxRows; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                var id = new Vector2Int(x, y);
                var localPos = new Vector3(x * config.SpacingBetweenPaths, 0, y);
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

    public void FillRowData(float pathValue, int rowId)
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