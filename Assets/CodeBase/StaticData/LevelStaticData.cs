using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "StaticData/Level")]
    public class LevelStaticData : ScriptableObject
    {
        [Header("Common settings")]
        public float SpacingBetweenPaths = 3f;
        public float StrafeAnimationTime = 0.5f;
        public float ScrollSpeed = 5f;

        [Space(10)]
        [Header("Generation settings")]
        public int ChunkLengthZ = 50;
        public float ChunkSideBorders = 1.5f;
        public int PreloadedChunks = 2;
        public float SafeRunZone = 30f;
        public float PerlinScale = 20f;
    }
}