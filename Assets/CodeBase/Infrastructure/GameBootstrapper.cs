using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Infrastructure
{
  public class GameBootstrapper : MonoBehaviour
  {
    [SerializeField] 
    private LoadingCurtain _loadingCurtainPrefab;

    [SerializeField]
    private TickableManager _tickableManagerPrefab;
    
    private Game _game;

    private void Awake()
    {
      
      var singleUpdateGameobject = FindObjectOfType<TickableManager>();

      if (singleUpdateGameobject == null)
      {
        singleUpdateGameobject = Instantiate(_tickableManagerPrefab);
      }

      _game = new Game(singleUpdateGameobject, singleUpdateGameobject,Instantiate(_loadingCurtainPrefab));
      _game.StateMachine.Enter<BootstrapState>();

      DontDestroyOnLoad(this);
    }
  }
}