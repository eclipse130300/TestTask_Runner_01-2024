using System.Collections.Generic;
using System.Linq;
using CodeBase.Chunks;
using CodeBase.Hero;
using CodeBase.Infrastructure.AssetManagement;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private GameObject _heroGameObject;
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _persistentProgressService;

        public List<ISavedProgressReader> ReadersList { get; } = new List<ISavedProgressReader>();

        public List<ISavedProgressWriter> WritersList { get; } = new List<ISavedProgressWriter>();

        public GameFactory(
            IAssetProvider assetProvider,
            IStaticDataService staticDataService,
            IPersistentProgressService persistentProgressService)
        {
            _assetProvider = assetProvider;
            _staticData = staticDataService;
            _persistentProgressService = persistentProgressService;
        }

        public GameObject CreateHero(GameObject at)
        {
            _heroGameObject = InstantiateRegistered(AssetPath.HERO_PATH, at.transform.position);
            return _heroGameObject;
        }

        public GameObject CreateHud()
        {
            GameObject hud = InstantiateRegistered(AssetPath.HUD_PATH);

            return hud;
        }

        public GameObject CreateGroundChunk(Vector3 at, Vector3 forward, Vector3 scale)
        {
            var chunk = _assetProvider.Instantiate(AssetPath.GROUND_CHUNK_PATH);
            chunk.transform.position = at;
            chunk.transform.forward = forward;

            var chunkController = chunk.GetComponent<ChunkController>();
            //we need to add obstacles under this transform, so lets not mess up with scale of root
            chunkController.ChunkVisualTransform.localScale = scale;
            chunkController.Construct(_staticData);

            return chunk;
        }
        
        public GameObject CreateObstacle(Transform parent, Vector3 localPos)
        {
            var obstacle = _assetProvider.Instantiate(AssetPath.OBSTACLE_PATH);
            
            var levelData = _staticData.ForGame();
            
            obstacle.transform.SetParent(parent);
            obstacle.transform.localPosition = localPos;
            obstacle.transform.localScale = new Vector3(levelData.LinesSpacingX, levelData.LinesSpacingX, levelData.LinesSpacingZ);
            
            return obstacle;
        }

        //as we do not care of power up type, just use random
        public GameObject CreatePowerUp(Transform parent, Vector3 localPos)
        {
            var randomPowerUpPath = _staticData
                                   .ForGame()
                                   .PowerUpsPathsPool
                                   .OrderBy(_ => new System.Random().Next())
                                   .Take(1)
                                   .SingleOrDefault();

            var powerUp = _assetProvider.Instantiate(randomPowerUpPath);
            
            powerUp.transform.SetParent(parent);
            powerUp.transform.localPosition = localPos;
            
            return powerUp;
        }

        public void CleanUp()
        {
            ReadersList?.Clear();
            WritersList?.Clear();
        }

        private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
        {
            var hero = _assetProvider.Instantiate(prefabPath, at: at);
            RegisterProgressWriters(fromGameObject: hero);
            return hero;
        }
        
        private GameObject InstantiateRegistered(string prefabPath)
        {
            var instantiatedPrefab = _assetProvider.Instantiate(prefabPath);
            RegisterProgressWriters(fromGameObject: instantiatedPrefab);
            return instantiatedPrefab;
        }
        
        private void RegisterProgressWriters(GameObject fromGameObject)
        {
            foreach (ISavedProgressReader reader in fromGameObject.GetComponentsInChildren<ISavedProgressReader>())
            {
                Register(reader);
            }
        }

        private void Register(ISavedProgressReader reader)
        {
            if (reader is ISavedProgressWriter writer)
            {
                WritersList.Add(writer);
            }
            
            ReadersList.Add(reader);
        }
    }
}