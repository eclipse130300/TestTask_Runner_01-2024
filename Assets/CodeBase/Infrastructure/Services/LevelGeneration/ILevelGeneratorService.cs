using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Services
{
    public interface ILevelGeneratorService : IService
    {
        public void InitializeLevel();
        public Queue<LevelChunk> GeneratedChunks { get; }
    }
}