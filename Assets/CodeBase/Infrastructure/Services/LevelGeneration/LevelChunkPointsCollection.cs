using System.Collections.Generic;

namespace CodeBase.Services
{
    //actually might be any collection<T> but whatever
    public class LevelChunkPointsCollection
    {
        public readonly List<ChunkSamplePoint> PointsCollection;

        public LevelChunkPointsCollection(List<ChunkSamplePoint> pointsCollection)
        {
            PointsCollection = pointsCollection;
        }
    }
}