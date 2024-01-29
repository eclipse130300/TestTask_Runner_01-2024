using System;
using JetBrains.Annotations;
using UnityEngine;

public class WindowInfo
{
    public Type Type;
    public GameObject GameObject;
    [CanBeNull]
    public IWindowAnimator Animator;

    public WindowInfo(Type type, GameObject gameObject, [CanBeNull] IWindowAnimator animator)
    {
        Type = type;
        GameObject = gameObject;
        Animator = animator;
    }
    
    public bool HasAnimator() => Animator != null;
}