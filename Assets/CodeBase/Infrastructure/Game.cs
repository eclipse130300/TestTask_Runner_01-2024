using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using CodeBase.Services;

namespace CodeBase.Infrastructure
{
  /// <summary>
  /// entry point to a game as a POCO
  /// </summary>
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