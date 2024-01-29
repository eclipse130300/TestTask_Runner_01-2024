using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.StaticData;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    /// <summary>
    /// state used for initialization & registration of services
    /// </summary>
    public class BootstrapState : IState
    {
        private const string INITIAL_SCENE = "Initial";
        
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;
        private readonly TickableService _tickableService;

        public BootstrapState(GameStateMachine stateMachine, TickableService tickableService, SceneLoader sceneLoader, AllServices services)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            _tickableService = tickableService;
            
            RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.Load(INITIAL_SCENE, onLoad: LoadProgress);
        }
        
        public void Exit()
        {
        }

        private void LoadProgress() 
            => _stateMachine.Enter<LoadProgressState>();
        
        private void RegisterServices()
        {
            RegisterStaticData();
            _services.RegisterSingle<ICoroutineRunnerService>(_tickableService);
            _services.RegisterSingle<IInputService>(GetPlatformInput());
            _services.RegisterSingle<IAssetProvider>(new AssetProvider());
            _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
            _services.RegisterSingle<IUIService>
                (new UIService(_services.Single<IAssetProvider>(), _services.Single<IPersistentProgressService>()));

            _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssetProvider>(),
                _services.Single<IStaticDataService>(),
                _services.Single<IPersistentProgressService>()));

            _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(_services.Single<IPersistentProgressService>(),
                _services.Single<IGameFactory>()));

            _services.RegisterSingle<ILevelGeneratorService>(new LevelGeneratorService(_services.Single<IStaticDataService>(),
                _services.Single<IGameFactory>()));

            var levelRunner = new LevelRunnerService(_services.Single<ILevelGeneratorService>(), _services.Single<IStaticDataService>());
            _tickableService.Register(levelRunner);
            _services.RegisterSingle<ILevelRunnerService>(levelRunner);
        }

        private void RegisterStaticData()
        {
            IStaticDataService staticDataService = new StaticDataService();
            staticDataService.Load();
            _services.RegisterSingle(staticDataService);
        }

        private static IInputService GetPlatformInput()
        {
            if (Application.isEditor)
                return new StandaloneInputService();
      
            return new MobileInputService();
        }
    }
}