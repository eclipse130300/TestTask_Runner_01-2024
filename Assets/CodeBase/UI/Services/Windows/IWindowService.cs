﻿namespace CodeBase.UI.Services.Windows
{
    public interface IWindowService : IService
    {
        void Open(WindowType windowType);
    }
}