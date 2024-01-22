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
        
        
        private LevelStaticData _levelStaticData;
        private Dictionary<WindowType,WindowConfig> _windowConfigs;

        public void Load()
        {
            _levelStaticData = Resources.Load<LevelStaticData>(STATICDATA_LEVELS);

            _windowConfigs = Resources.Load<WindowsStaticData>(STATICDATA_WINDOWS).Configs
                                      .ToDictionary(x => x.WindowType, x => x);
        }

        public LevelStaticData ForLevel() => _levelStaticData;

        public WindowConfig ForWindow(WindowType windowType) =>
            _windowConfigs.TryGetValue(windowType, out WindowConfig staticData) 
                ? staticData
                : null;
    }
}