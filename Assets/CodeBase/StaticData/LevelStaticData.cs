using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "StaticData/Level")]
    public class LevelStaticData : ScriptableObject
    {
        [Header("Common settings")]
        public float LinesSpacingX = 3f;
        public float LinesSpacingZ = 3f;

        public float StrafeAnimationTime = 0.5f;
        public float ScrollSpeed = 5f;

        [Space(10)]
        [Header("Generation settings")]
        public int ChunkRows = 50;
        public float ChunkSideBorders = 1.5f;
        public int PreloadedChunks = 2;
        public int SafeRunZone = 30;
        public float PerlinScale = 20f;

        public float ObstaclePercent = 0;
        public int DontSpawnObstaclesOnStartRowsAmount = 3;

        [Header("PowerUps")]
        public int ChunkPowerUpsAmount = 2;

        public List<string> PowerUpsPathsPool = new();
    }
}