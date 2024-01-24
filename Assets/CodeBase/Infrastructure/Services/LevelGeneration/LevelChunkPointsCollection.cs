using System.Collections.Generic;

public class LevelChunkPointsCollection
{
    public readonly IReadOnlyCollection<ChunkSamplePoint> PointsCollection;

    public LevelChunkPointsCollection(List<ChunkSamplePoint> pointsCollection)
    {
        PointsCollection = pointsCollection;
    }
}