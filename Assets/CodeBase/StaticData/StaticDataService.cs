using UnityEngine;

namespace CodeBase.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string STATICDATA_GAME = "StaticData/GameData";
        
        private GameStaticData gameStaticData;

        public void Load()
        {
            gameStaticData = Resources.Load<GameStaticData>(STATICDATA_GAME);
        }

        public GameStaticData ForGame() => gameStaticData;
    }
}