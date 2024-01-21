using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Infrastructure
{
  public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
  {
    public LoadingCurtain LoadingCurtainPrefab;
    
    private Game _game;

    private void Awake()
    {
      _game = new Game(this, Instantiate(LoadingCurtainPrefab));
      _game.StateMachine.Enter<BootstrapState>();

      DontDestroyOnLoad(this);
    }
  }
}