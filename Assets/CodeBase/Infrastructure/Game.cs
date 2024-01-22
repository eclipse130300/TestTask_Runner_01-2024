using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services.Input;

namespace CodeBase.Infrastructure
{
  public class Game
  {
    public readonly GameStateMachine StateMachine;

    public Game(ICoroutineRunnerService coroutineRunnerService, TickableManager tickableManager, LoadingCurtain loadingCurtain)
    {
       StateMachine = new GameStateMachine(new SceneLoader(coroutineRunnerService),
         tickableManager,
           loadingCurtain,
           AllServices.Container);
    }
  }
}