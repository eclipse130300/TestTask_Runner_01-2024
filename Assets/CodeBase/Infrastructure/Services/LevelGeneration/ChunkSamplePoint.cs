using System.Collections.Generic;
using UnityEngine;

public class ChunkSamplePoint
{
    public readonly Vector2Int Id;
    public readonly Vector3 LocalPosition;

    public SamplePointType SamplePointType = SamplePointType.None;
    
    public readonly List<ChunkSamplePoint> AdjacentChunks = new();

    public ChunkSamplePoint(Vector2Int id, Vector3 localPosition)
    {
        Id = id;
        LocalPosition = localPosition;
    }

}