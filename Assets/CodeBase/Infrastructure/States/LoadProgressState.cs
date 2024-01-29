using CodeBase.Services;

namespace CodeBase.Infrastructure.States
{
    /// <summary>
    /// State used for loading and applying persistent data to game world
    /// </summary>
    ///Now is empty, but could be used with ease for future
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadService;

        public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, ISaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _saveLoadService = saveLoadService;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            _gameStateMachine.Enter<LoadLevelState, string>("Main");
        }

        public void Exit()
        {

        }

        private void LoadProgressOrInitNew() => 
            _progressService.Progress 
            = _saveLoadService.LoadProgress() 
           ?? NewProgress();

        private PlayerProgress NewProgress()
        {
           var progress = new PlayerProgress();
           return progress;
        }
    }
}