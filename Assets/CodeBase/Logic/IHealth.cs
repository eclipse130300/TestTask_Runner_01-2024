using System;

public interface IHealth
{
    event Action OnHealthChanged;
    float Current { get; set; }
    float Max { get; set; }
    void TakeDamage(float damage);
}