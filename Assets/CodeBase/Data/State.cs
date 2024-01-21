using System;

[Serializable]
public class State
{
    public float CurrentHp;
    public float MaxHp;

    public void ResetHP()
        => CurrentHp = MaxHp;
}