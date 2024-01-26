using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.Input;
using CodeBase.StaticData;
using CodeBase.UI.Services;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string INITIAL_SCENE = "Initial";
        
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;
        private readonly TickableManager _tickableManager;

        public BootstrapState(GameStateMachine stateMachine, TickableManager tickableManager, SceneLoader sceneLoader, AllServices services)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            _tickableManager = tickableManager;
            
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
            _services.RegisterSingle<ICoroutineRunnerService>(_tickableManager);
            _services.RegisterSingle<IInputService>(GetPlatformInput());
            _services.RegisterSingle<IAssetProvider>(new AssetProvider());
            _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
            _services.RegisterSingle<IUIFactory>
                (new UIFactory(_services.Single<IAssetProvider>(), _services.Single<IStaticDataService>(), _services.Single<IPersistentProgressService>()));
            _services.RegisterSingle<IWindowService>
                (new WindowService(_services.Single<IUIFactory>()));
            
            _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IWindowService>(), _services.Single<IAssetProvider>(),
                _services.Single<IStaticDataService>(),
                _services.Single<IPersistentProgressService>()));

            _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(_services.Single<IPersistentProgressService>(),
                _services.Single<IGameFactory>()));

            _services.RegisterSingle<ILevelGeneratorService>(new LevelGeneratorService(_services.Single<IStaticDataService>(),
                _services.Single<IGameFactory>()));

            var levelRunner = new LevelRunnerService(_services.Single<ILevelGeneratorService>(), _services.Single<IStaticDataService>());
            _tickableManager.Register(levelRunner);
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