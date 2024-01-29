using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.UI
{
    /// <summary>
    /// Service responsible for ladoing and caching UI Windows 
    /// </summary>
    public class UIService : IUIService
    {
        private const string BasePath = "Windows/";
        private const string UI_ROOT_PATH = "UI/UI_Root";
        
        private readonly IAssetProvider _assets;
        private readonly IPersistentProgressService _progressService;
        private Transform _root;

        private List<WindowInfo> _intstantiatedList = new ();

        public UIService(IAssetProvider assets, IPersistentProgressService progressService)
        {
            _assets = assets;
            _progressService = progressService;
        }

        public void CreateUIRoot() => 
            _root = _assets.Instantiate(UI_ROOT_PATH).transform;
        
        
        public async UniTask<bool> ShowPopup<T>() where T : BaseWindow
        {
            Debug.Log($"Showing popup {typeof(T).Name}");
            var tcs = new TaskCompletionSource<bool>();

            var instantiated = await HandleIntatntiatedPopupLoad<T>(tcs);
            if(instantiated)
                return true;
            
            HandleAsyncPopupLoad<T>(tcs);

            return await tcs.Task;
        }
        
        public async UniTask<bool> HidePopup<T>() where T : BaseWindow
        {
            Debug.Log($"Hiding popup {typeof(T).Name}");
            var tcs = new TaskCompletionSource<bool>();
            
            if(TryGetInstantiated(typeof(T), out var windowInfo))
            {
                await HideLogic(windowInfo);
                tcs.TrySetResult(true);
            }
            else
            {
                return false;
            }
            
            return await tcs.Task;
        }

        private async Task<bool> HandleIntatntiatedPopupLoad<T>(TaskCompletionSource<bool> tcs) where T :  BaseWindow
        {
            var type = typeof(T);
            
            if (TryGetInstantiated(type, out var windowInfo))
            {
                //set as last sibling
                windowInfo.GameObject.transform.SetAsLastSibling();
                
                await ShowLogic(windowInfo);
                tcs.TrySetResult(true);

                return true;
            }

            return false;
        }

        private void HandleAsyncPopupLoad<T>(TaskCompletionSource<bool> tcs) where T : BaseWindow
        {
            var type = typeof(T);
            var path = Path.Combine(BasePath, type.Name);
            var asyncLoad = Resources.LoadAsync<T>(path);

            asyncLoad.completed += async operation =>
            {
                var prefab = asyncLoad.asset as T;
                if (prefab == null)
                    Debug.LogError($"Popup {type.Name} not found");

                T instance = Object.Instantiate(prefab, _root);
                var animator = instance.GetComponent<IWindowAnimator>();
                var windowInfo = new WindowInfo(typeof(T), instance.gameObject, animator);

                _intstantiatedList.Add(windowInfo);

                await ShowLogic(windowInfo);
                tcs.TrySetResult(true);
            };
        }

        private static async UniTask ShowLogic(WindowInfo windowInfo)
        {
            windowInfo.GameObject.SetActive(true);
            
            if(windowInfo.HasAnimator())
                await windowInfo.Animator.AnimateShow();
        }

        private static async UniTask HideLogic(WindowInfo windowInfo)
        {
            if(windowInfo.HasAnimator())
                await windowInfo.Animator.AnimateHide();
            
            windowInfo.GameObject.SetActive(false);
        }

        private bool TryGetInstantiated(Type type, out WindowInfo info)
        {
            var contains = _intstantiatedList.Any(x => x.Type == type);
            if (!contains)
            {
                info = null;
                return false;
            }
            
            var windowInfo = _intstantiatedList.Find(x => x.Type == type);
            info = windowInfo;
            return true;
        }

        public void CleanUp() => 
            _intstantiatedList.Clear();
    }
}