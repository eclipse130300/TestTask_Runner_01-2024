using System.Collections.Generic;

//actually might be any collection
public class LevelChunkPointsCollection
{
    public readonly List<ChunkSamplePoint> PointsCollection;

    public LevelChunkPointsCollection(List<ChunkSamplePoint> pointsCollection)
    {
        PointsCollection = pointsCollection;
    }
}