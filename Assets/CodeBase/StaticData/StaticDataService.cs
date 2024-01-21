using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string STATICDATA_LEVELS = "StaticData/Levels";
        private const string STATICDATA_WINDOWS = "StaticData/Windows/WindowsStaticData";
        
        
        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<WindowType,WindowConfig> _windowConfigs;

        public void Load()
        {
            _levels = Resources.LoadAll<LevelStaticData>(STATICDATA_LEVELS)
                               .ToDictionary(x => x.LevelKey, x => x);

            _windowConfigs = Resources.Load<WindowsStaticData>(STATICDATA_WINDOWS).Configs
                                      .ToDictionary(x => x.WindowType, x => x);
        }
        
        public LevelStaticData ForLevel(string sceneKey) =>  
            _levels.TryGetValue(sceneKey, out LevelStaticData staticData) 
                ? staticData
                : null;

        public WindowConfig ForWindow(WindowType windowType) =>
            _windowConfigs.TryGetValue(windowType, out WindowConfig staticData) 
                ? staticData
                : null;
    }
}