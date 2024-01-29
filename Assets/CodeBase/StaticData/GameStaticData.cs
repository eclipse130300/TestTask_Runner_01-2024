using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "StaticData/Level")]
    //we could make separate configs, but for test proj, whatever
    public class GameStaticData : ScriptableObject
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

        public float ObstaclePercentInChunk = 0;
        public int DontSpawnObstaclesOnStartRowsAmount = 3;

        [Header("PowerUps")]
        public int ChunkPowerUpsAmount = 2;

        public List<string> PowerUpsPathsPool = new();

        public float PowerUpDuration = 10f;

        public float SpeedUpModifier = 2f;
        public float SpeedDownModifier = 3f;

        public float FlyingHeight = 3f;
        public float TakeOffTime = 2f;
        
        [Header("UISettings")]
        public float UiAnimationTime = 0.5f;
        public float UiMinAlpha = 0.3f;
        public float UiMaxScale = 1.2f;
    }
}