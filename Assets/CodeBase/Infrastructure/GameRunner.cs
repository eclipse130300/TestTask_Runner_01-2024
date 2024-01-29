using UnityEngine;

namespace CodeBase.Infrastructure
{
    /// <summary>
    /// Monobehaviour used to run game from every scene
    /// </summary>
    public class GameRunner : MonoBehaviour
    {
        [SerializeField]
        private GameBootstrapper _gameBootstrapper;
        private void Awake()
        {
            var bootstrapper = FindObjectOfType<GameBootstrapper>();

            if (bootstrapper == null)
            {
                Instantiate(_gameBootstrapper);
            }
        }
    }
}