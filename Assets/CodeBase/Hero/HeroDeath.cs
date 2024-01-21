using System;
using CodeBase.Hero;
using UnityEngine;

public class HeroDeath : MonoBehaviour
{
    [SerializeField]
    private HeroMove _heroMove;

    [SerializeField]
    private HeroAnimator _heroAnimator;

    [SerializeField]
    private GameObject _deathFX;

    private bool _isDead;
    
    //make event - OnStumble (gameobject go) and check if object implements IDamagable 

    /*private void Start() => 
        _heroHealth.OnHealthChanged += HealthChanged;

    private void OnDestroy() =>
        _heroHealth.OnHealthChanged -= HealthChanged;*/

    private void HealthChanged()
    {
        if (CanDie())
            Die();
    }

    private bool CanDie() => !_isDead;

    private void Die()
    {
        _isDead = true;
        
        _heroMove.enabled = false;
        
        _heroAnimator.PlayDeath();

        Instantiate(_deathFX, transform.position, Quaternion.identity);
    }
}