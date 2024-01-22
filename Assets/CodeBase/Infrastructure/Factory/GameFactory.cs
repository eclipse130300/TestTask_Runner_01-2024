using System.Collections.Generic;
using CodeBase.Hero;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private GameObject _heroGameObject;
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly IWindowService _windowService;

        public List<ISavedProgressReader> ReadersList { get; } = new List<ISavedProgressReader>();

        public List<ISavedProgressWriter> WritersList { get; } = new List<ISavedProgressWriter>();

        public GameFactory(IWindowService windowService, IAssetProvider assetProvider, IStaticDataService staticDataService, IPersistentProgressService persistentProgressService)
        {
            _windowService = windowService;
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
            hud.GetComponentInChildren<LootCounter>().Construct(_persistentProgressService.Progress.WorldData);

            foreach (OpenWindowButton openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>())
                openWindowButton.Construct(_windowService);

            return hud;
        }

        public GameObject CreateGroundChunk(Vector3 at, Vector3 forward, Vector3 scale)
        {
            var chunk = _assetProvider.Instantiate(AssetPath.GROUND_CHUNK_PATH);
            chunk.transform.position = at;
            chunk.transform.forward = forward;
            chunk.transform.localScale = scale;

            return chunk;
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