using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services;

namespace CodeBase.Infrastructure
{
  public class Game
  {
    public readonly GameStateMachine StateMachine;

    public Game(ICoroutineRunnerService coroutineRunnerService, TickableService tickableManager, LoadingCurtain loadingCurtain)
    {
       StateMachine = new GameStateMachine(new SceneLoader(coroutineRunnerService),
         tickableManager,
           loadingCurtain,
           AllServices.Container);
    }
  }
}