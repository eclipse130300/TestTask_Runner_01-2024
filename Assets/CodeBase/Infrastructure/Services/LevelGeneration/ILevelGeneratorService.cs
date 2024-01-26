using UnityEngine;

public interface ILevelGeneratorService : IService
{
    public void InitializeLevel();
    float ChunkUnitySizeZ { get; }
    Vector3 FirstChunkSequencePosition { get; }
}