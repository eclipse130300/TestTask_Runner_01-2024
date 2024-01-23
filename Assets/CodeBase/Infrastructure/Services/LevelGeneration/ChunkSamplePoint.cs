using System.Collections.Generic;

public class ChunkSamplePoint
{
    public int IdX;
    public int IdY;
    public List<ChunkSamplePoint> AdjacentChunks = new();
    public bool IsPathPoint;
}