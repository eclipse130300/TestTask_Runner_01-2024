using System.Collections.Generic;
using UnityEngine;

public interface ILevelGeneratorService : IService
{
    public void InitializeLevel();
    public Queue<LevelChunk> GeneratedChunks { get; }
}