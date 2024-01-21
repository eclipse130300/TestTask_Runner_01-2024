using System;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;

namespace CodeBase.StaticData.Windows
{
    [Serializable]
    public class WindowConfig
    {
        public WindowType WindowType;
        public WindowBase Prefab;
    }
}