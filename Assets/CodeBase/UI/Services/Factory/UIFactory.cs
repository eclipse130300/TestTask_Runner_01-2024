using CodeBase.Infrastructure.AssetManagement;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;
using UnityEngine;

namespace CodeBase.UI.Services
{
    public class UIFactory : IUIFactory
    {
        private const string UI_ROOT_PATH = "UI/UI_Root";
        
        private readonly IAssetProvider _assets;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;
        private Transform _root;


        public UIFactory(IAssetProvider assets, IStaticDataService staticData, IPersistentProgressService progressService)
        {
            _assets = assets;
            _staticData = staticData;
            _progressService = progressService;
        }

        public void CreateUIRoot() => 
            _root = _assets.Instantiate(UI_ROOT_PATH).transform;
    }
}