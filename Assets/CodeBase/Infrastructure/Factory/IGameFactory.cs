using System.Collections.Generic;
using CodeBase.Hero;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        GameObject CreateHero(GameObject at);
        GameObject CreateHud();
        List<ISavedProgressReader> ReadersList { get; }
        List<ISavedProgressWriter> WritersList { get; }
        void CleanUp();
        GameObject CreateGroundChunk(Vector3 at, Vector3 forward, Vector3 scale);
        GameObject CreateObstacle(Transform parent, Vector3 localPos);
    }
}