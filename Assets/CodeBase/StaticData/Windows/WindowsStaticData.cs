using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.StaticData.Windows
{
    [CreateAssetMenu(fileName = "StaticData/Window static data", menuName = "WindowStaticData")]

    public class WindowsStaticData : ScriptableObject
    {
        public List<WindowConfig> Configs;
    }
}