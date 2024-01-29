using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.StaticData;
using EventBusSystem;
using Events;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Services
{
    /// <summary>
    /// Service that invokes generation of chunks procedurally and spawns level chunks on the level
    /// Also generates additional chunks as they disappear from player view
    /// </summary>
    public class LevelGeneratorService : ILevelGeneratorService, IChunkReadyForUnloadHandler, IDisposable
    {
        public Queue<LevelChunk> GeneratedChunks => _generatedChunks;

        private readonly IStaticDataService _staticDataService;
        private readonly IGameFactory _gameFactory;
        private readonly Queue<LevelChunk> _generatedChunks = new();

        private Vector3 _firstChunkSequencePosition;
        private Vector3 _lastChunkSequencePosition;

        private int _currentRow;
        private float _randomSeedPerlinOffset;

        public LevelGeneratorService(IStaticDataService staticDataService, IGameFactory gameFactory)
        {
            _staticDataService = staticDataService;
            _gameFactory = gameFactory;

            EventBus.Subscribe(this);
        }

        public void InitializeLevel()
        {
            CleanUp();
            var levelData = _staticDataService.ForGame();

            _randomSeedPerlinOffset = Random.Range(-100f, 100f);

            _firstChunkSequencePosition = Vector3.zero;
            _lastChunkSequencePosition = _firstChunkSequencePosition + Vector3.forward * levelData.ChunkRows * levelData.LinesSpacingZ * (levelData.PreloadedChunks - 1);

            var currentPos = _firstChunkSequencePosition;

            for (int i = 0; i < levelData.PreloadedChunks; i++)
            {
                CreateChunk(levelData, currentPos);
                currentPos += Vector3.forward * levelData.ChunkRows * levelData.LinesSpacingZ;
            }
        }

        private void CreateChunk(GameStaticData staticData, Vector3 chunkPosition)
        {
            var chunkXSize = (staticData.LinesSpacingX + staticData.ChunkSideBorders) * 2;
            var chunkZSize = staticData.LinesSpacingZ * staticData.ChunkRows;
            var scale = new Vector3(chunkXSize, 1, chunkZSize);
            var groundChunk = _gameFactory.CreateGroundChunk(chunkPosition, Vector3.forward, scale);
            var chunkData = new LevelChunk(staticData, groundChunk);

            _generatedChunks.Enqueue(chunkData);

            //path is generated using redistributed Perlin noise
            GenerateChunkPlayerPath(staticData, chunkData);

            //obstacles are generated on empty spaces(not path points) using Breadth-first search and picking the biggest of them
            chunkData.InitializeObstaclesData();
            SpawnObstacles(chunkData, groundChunk.transform);

            //powerUps positions are randomly picked out of path points
            chunkData.InitializePowerUpsData();
            SpawnPowerUps(chunkData, groundChunk.transform);
        }

        private void SpawnPowerUps(LevelChunk chunkData, Transform chunkTransform)
        {
            foreach (var powerUpPoint in chunkData.PowerUps)
            {
                _gameFactory.CreatePowerUp(chunkTransform, powerUpPoint.LocalPosition);
            }
        }

        private void GenerateChunkPlayerPath(GameStaticData staticData, LevelChunk chunk)
        {
            var maxRows = staticData.ChunkRows;

            for (int i = 0; i < staticData.ChunkRows; i++)
            {
                _currentRow++;

                if (_currentRow < staticData.SafeRunZone)
                {
                    chunk.DeleteRowData(i);
                    continue;
                }

                var perlinScale = staticData.PerlinScale;

                var randomPathVal01 = Mathf.PerlinNoise((_currentRow + _randomSeedPerlinOffset) * perlinScale, 0);
                var discretePathVal = Redistribute(randomPathVal01);

                chunk.FillRowPathData(discretePathVal, i);
            }
        }

        private void SpawnObstacles(LevelChunk chunkData, Transform chunkTransform)
        {
            foreach (var obstacle in chunkData.Obstacles)
            {
                foreach (var obstaclePoint in obstacle.PointsCollection)
                    _gameFactory.CreateObstacle(chunkTransform, obstaclePoint.LocalPosition);
            }
        }

        //redistribute value for better path generation
        //final value is in -1 to 1 range for convenience
        private float Redistribute(float val01)
        {
            var rangeBias = 0.2f;
            var remapped = val01.Remap(0f, 1f, -1f, 1f);

            if (WithinUnsignedRange(remapped, rangeBias))
                return 0;

            return Mathf.Sign(remapped);
        }

        private bool WithinUnsignedRange(float val, float range) => Mathf.Abs(val) <= range;

        public void OnChunkReadyForUnload(float overshootOffset)
        {
            //unload first chunk
            var firstChunk = _generatedChunks.Dequeue();
            GameObject.Destroy(firstChunk.ViewGameObject);

            //create new chunk
            var overshotPosition = new Vector3(_lastChunkSequencePosition.x,
                _lastChunkSequencePosition.y,
                _lastChunkSequencePosition.z + overshootOffset);

            CreateChunk(_staticDataService.ForGame(), overshotPosition);
        }

        private void CleanUp()
        {
            foreach (var chunk in _generatedChunks)
                GameObject.Destroy(chunk.ViewGameObject);

            _generatedChunks.Clear();
            _currentRow = 0;
        }

        public void Dispose()
        {
            EventBus.Unsubscribe(this);
            CleanUp();
        }
    }
}