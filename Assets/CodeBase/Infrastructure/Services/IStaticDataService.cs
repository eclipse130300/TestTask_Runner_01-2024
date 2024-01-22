﻿using CodeBase.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;

public interface IStaticDataService : IService
{
    void Load();
    LevelStaticData ForLevel();
    WindowConfig ForWindow(WindowType shop);
}