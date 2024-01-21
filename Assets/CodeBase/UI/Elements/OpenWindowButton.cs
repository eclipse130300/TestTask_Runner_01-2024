using System;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.UI;

public class OpenWindowButton : MonoBehaviour
{
    [SerializeField]
    private Button _button;

    [SerializeField]
    private WindowType _windowType;
    
    private IWindowService _windowService;

    public void Construct(IWindowService windowService) => 
        _windowService = windowService;

    private void Awake() => 
        _button.onClick.AddListener(Open);

    private void Open() => 
        _windowService.Open(_windowType);
}