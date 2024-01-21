using CodeBase.CameraLogic;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string INITIALPOINT_TAG = "InitialPoint";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IStaticDataService _staticDataService;
        private IUIFactory _uiFactory;

        public LoadLevelState(
            GameStateMachine stateMachine,
            SceneLoader sceneLoader,
            LoadingCurtain loadingCurtain,
            IGameFactory gameFactory,
            IPersistentProgressService progressService,
            IStaticDataService staticDataService,
            IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _staticDataService = staticDataService;
        }

        public void Enter(string loadSceneName)
        {
            _loadingCurtain.Show();
            _gameFactory.CleanUp();
            _sceneLoader.Load(loadSceneName, OnLoad);
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

            _stateMachine.Enter<GameLoopState>();
        }

        private void InitUIRoot() => 
            _uiFactory.CreateUIRoot();

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.ReadersList)
                progressReader.LoadProgress(_progressService.Progress);
        }

        private void InitGameWorld()
        {
            InitSpawners();
            
            var hero = InitHero();

            InitHud(hero);
            CameraFollow(hero);
        }

        private void InitSpawners()
        {
            string sceneKey = SceneManager.GetActiveScene().name;
            LevelStaticData levelData = _staticDataService.ForLevel(sceneKey);
        }

        private GameObject InitHero()
        {
            var initialPoint = GameObject.FindWithTag(INITIALPOINT_TAG);
            var hero = _gameFactory.CreateHero(at: initialPoint);
            return hero;
        }

        private void InitHud(GameObject hero)
        {
            GameObject hud = _gameFactory.CreateHud();
            
            hud.GetComponentInChildren<ActorUI>()
               .Construct(hero.GetComponent<IHealth>());
        }

        private void CameraFollow(GameObject hero) =>
            Camera.main.GetComponent<CameraFollow>().Follow(hero);
    }
}