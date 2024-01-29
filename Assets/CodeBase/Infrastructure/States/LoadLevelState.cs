using CodeBase.CameraLogic;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.UI.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticDataService;
        private readonly IUIService iuiService;
        private readonly ILevelGeneratorService _levelGeneratorService;

        public LoadLevelState(
            GameStateMachine stateMachine,
            SceneLoader sceneLoader,
            LoadingCurtain loadingCurtain,
            TickableManager tickableManager,
            IGameFactory gameFactory,
            IPersistentProgressService progressService,
            IStaticDataService staticDataService,
            IUIService iuiService,
            ILevelGeneratorService levelGeneratorService)
        {
            this.iuiService = iuiService;
            this._levelGeneratorService = levelGeneratorService;
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _staticDataService = staticDataService;
        }

        public void Enter(string payload)
        {
            _loadingCurtain.Show();
            _gameFactory.CleanUp();
            _sceneLoader.Load(payload, OnLoad);
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
        }

        private void OnLoad()
        {
            InitUIRoot();
            InitGameWorld();
            InformProgressReaders();

            _stateMachine.Enter<GameplayPausedState, bool>(true);
        }

        private void InitUIRoot() => 
            iuiService.CreateUIRoot();

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.ReadersList)
                progressReader.LoadProgress(_progressService.Progress);
        }

        private void InitGameWorld()
        {
            InitHud();
            GenerateLevel();
            
            var hero = InitHero();
            CameraFollow(hero);
        }

        private void GenerateLevel() => 
            _levelGeneratorService.InitializeLevel();

        private GameObject InitHero()
        {
            var initialPoint = GameObject.FindWithTag(Constants.INITIALPOINT_TAG);
            var hero = _gameFactory.CreateHero(at: initialPoint);
            return hero;
        }

        private void InitHud()
        {
            GameObject hud = _gameFactory.CreateHud();
        }

        private void CameraFollow(GameObject hero) =>
            Camera.main.GetComponent<CameraStaticFollow>().Follow(hero);
    }
}