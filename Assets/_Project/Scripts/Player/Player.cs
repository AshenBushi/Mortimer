using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    private const int DefaultMaxHealth = 200;
    private const int DefaultDamage = 10;
    
    [SerializeField] private EnemySpawner _enemySpawner;
    [Space]
    [SerializeField] private PlayerStats _playerStats;

    private PlayerAttackHandler _playerAttackHandler;
    private Animator _animator;
    private UltimateDefense _ultimateDefense;
    public PlayerStats PlayerStats => _playerStats;
    public event UnityAction OnHealthChanged;

    private void Awake()
    {
        _playerAttackHandler = GetComponent<PlayerAttackHandler>();
        _ultimateDefense = GetComponentInChildren<UltimateDefense>();
        _animator = GetComponent<Animator>();
    }

    public void Init()
    {
        _playerStats = new PlayerStats(DefaultMaxHealth * PerksHandler.Instance.GetPerkBoost(PerkName.Health),
            DefaultDamage * PerksHandler.Instance.GetPerkBoost(PerkName.Damage));
        
        OnHealthChanged?.Invoke();
    }
    
    private void OnEnable()
    {
        _enemySpawner.OnEnemyKilled += OnEnemyKilled;
    }
    
    private void OnDisable()
    {
        _enemySpawner.OnEnemyKilled -= OnEnemyKilled;
    }

    private void Die()
    {
        _animator.SetTrigger("Died");
        _playerAttackHandler.Die();
        SessionManager.Instance.EndSession();
    }

    private void OnEnemyKilled(Enemy enemy)
    {
        User.Instance.AddMoney((int)(enemy.MoneyReward * PerksHandler.Instance.GetPerkBoost(PerkName.Income)));
    }

    public void TakeDamage(int damage)
    {
        if(_ultimateDefense.IsUltimateShield) return;

        if (_playerStats.DodgeChance > 0)
        {
            var randomValue = Random.Range(1, 101);
            
            if(randomValue <= _playerStats.DodgeChance) return;
        }
        
        _playerStats.Health -= damage;

        OnHealthChanged?.Invoke();

        if (_playerStats.Health <= 0)
        {
            Die();
        }
    }

    public void IncreaseHealth(int value)
    {
        _playerStats.MaxHealth += value;
        _playerStats.Health += value;
        
        OnHealthChanged?.Invoke();
    }

    public void IncreaseDamage(int value)
    {
        _playerStats.Damage += value;
    }

    public void SetAttackSpeed(float value)
    {
        _playerStats.AttackSpeed = value;
    }

    public void SetDodgeChance(int value)
    {
        _playerStats.DodgeChance = value;
    }
}

[Serializable]
public struct PlayerStats
{
    public int MaxHealth;
    public int Health;
    public int Damage;
    public float AttackSpeed;
    public int DodgeChance;

    public PlayerStats(float maxHealth, float damage)
    {
        MaxHealth = (int)maxHealth;
        Health = MaxHealth;
        Damage = (int)damage;
        AttackSpeed = 1;
        DodgeChance = 0;
    }
}
