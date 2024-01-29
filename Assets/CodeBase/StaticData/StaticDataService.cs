using UnityEngine;

namespace CodeBase.StaticData
{
    /// <summary>
    /// provides static data (configs) to any client. Can load Scriptable, Remote, Xml, etc under the hood
    /// </summary>
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