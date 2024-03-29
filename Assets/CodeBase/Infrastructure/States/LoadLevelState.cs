﻿using CodeBase.CameraLogic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.Services;
using CodeBase.UI;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    /// <summary>
    /// generates and instantiates all dynamic objects in the game world
    /// </summary>
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _progressService;
        private readonly IUIService _iuiService;
        private readonly ILevelGeneratorService _levelGeneratorService;

        public LoadLevelState(
            GameStateMachine stateMachine,
            SceneLoader sceneLoader,
            LoadingCurtain loadingCurtain,
            IGameFactory gameFactory,
            IPersistentProgressService progressService,
            IUIService iuiService,
            ILevelGeneratorService levelGeneratorService)
        {
            _iuiService = iuiService;
            _levelGeneratorService = levelGeneratorService;
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
        }

        public void Enter(string payload)
        {
            _loadingCurtain.Show();
            _iuiService.CleanUp();
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

            _stateMachine.Enter<GameplayState, bool>(true);
        }

        private void InitUIRoot()
        {
            _iuiService.CreateUIRoot();
        }

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