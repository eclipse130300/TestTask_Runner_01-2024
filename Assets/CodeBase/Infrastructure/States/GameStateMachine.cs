using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.UI.Services;

namespace CodeBase.Infrastructure.States
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type,IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, TickableManager tickableManager, LoadingCurtain loadingCurtain, AllServices services)
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this,tickableManager, sceneLoader, services),
                [typeof(LoadLevelState)] = new LoadLevelState(this,
                    sceneLoader,
                    loadingCurtain,
                    tickableManager,
                    services.Single<IGameFactory>(),
                    services.Single<IPersistentProgressService>(),
                    services.Single<IStaticDataService>(),
                    services.Single<IUIService>(),
                    services.Single<ILevelGeneratorService>()),
                
                [typeof(LoadProgressState)] = new LoadProgressState(this,
                    services.Single<IPersistentProgressService>(),
                    services.Single<ISaveLoadService>()),
                
                [typeof(GameplayPausedState)] = new GameplayPausedState(this),
            };
        }
    
        public void Enter<TState>() where TState : class, IState
        {
            var state = ChangeState<TState>();
            state.Enter();
        }
    
        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            var state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            var state = GetState<TState>();
            _activeState = state;
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }
    }
}