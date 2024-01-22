using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "StaticData/Level")]
    public class LevelStaticData : ScriptableObject
    {
        public float SpacingBetweenPaths = 3f;
        public float StrafeAnimationTime = 0.5f;
    }
}