using System;
using CodeBase.UI.Services;
using Cysharp.Threading.Tasks;
using EventBusSystem;
using Events;

namespace CodeBase.Infrastructure.States
{
    public class GameplayState : IPayloadedState<bool>, IGameplayStartedHandler, IGameplayFinishedHandler
    {
        private ILevelRunnerService _levelRunnerService;
        private IUIService _uiService;
        private GameStateMachine _gameStateMachine;
        
        public GameplayState(GameStateMachine stateMachine, ILevelRunnerService levelRunnerService, IUIService uiService)
        {
            _uiService = uiService;
            _levelRunnerService = levelRunnerService;
            _gameStateMachine = stateMachine;
        }

        public void Enter(bool payload)
        {
            EventBus.Subscribe(this);
            _uiService.ShowPopup<PausedGameWindow>();
        }

        public void Exit()
        {
            EventBus.Unsubscribe(this);
        }

        public void OnGameLoopStated()
        {
            _uiService.HidePopup<PausedGameWindow>();
            _levelRunnerService.Run();
        }

        public async void OnGameLoopFinished()
        {
            _levelRunnerService.Stop();
            
            //ugly way of reloading game
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            _gameStateMachine.Enter<BootstrapState>();

        }
    }
}