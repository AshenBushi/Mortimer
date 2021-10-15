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
    [Header("Active Skills")]
    [SerializeField] private List<ActiveSkill> _activeSkills;
    [SerializeField] private DoubleDamage _doubleDamage;
    [SerializeField] private UltimateDefense _ultimateDefense;
    [Header("Auras")]
    [SerializeField] private FireAura _fireAura;
    [SerializeField] private IceAura _iceAura;
    [Space]
    [SerializeField] private PlayerStats _playerStats;

    private PlayerStateHandler _playerStateHandler;
    private Animator _animator;
    
    public PlayerStats PlayerStats => _playerStats;
    public event UnityAction OnHealthChanged;

    private void Awake()
    {
        _playerStateHandler = GetComponent<PlayerStateHandler>();
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
        _playerStateHandler.Die();
        SessionManager.Instance.EndSession();
    }
    
    private void OnEnemyKilled(Enemy enemy)
    {
        Wallet.Instance.AddMoney((int)(enemy.MoneyReward * PerksHandler.Instance.GetPerkBoost(PerkName.Income)));
    }

    public void TakeDamage(int damage)
    {
        if(_ultimateDefense.IsUltimateShield || _playerStateHandler.IsBlocking) return;

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
    
    public void UpgradeStonePeaks(int value)
    {
        if(!_activeSkills[0].IsActive)
            _activeSkills[0].Enable();
        
        _activeSkills[0].SetCooldown(value);
    }
    
    public void UpgradeUltimateDefense(int value)
    {
        if(!_activeSkills[1].IsActive)
            _activeSkills[1].Enable();
        
        _ultimateDefense.SetDuration(value);
    }
    
    public void UpgradeDoubleDamage(int value)
    {
        if(!_activeSkills[2].IsActive)
            _activeSkills[2].Enable();
        
        _doubleDamage.SetDuration(value);
    }
    
    public void UpgradeFireAura(int value)
    {
        if(!_fireAura.gameObject.activeSelf)
            _fireAura.gameObject.SetActive(true);
        _fireAura.SetDamage(value);
    }
    
    public void UpgradeIceAura(float value)
    {
        if(!_iceAura.gameObject.activeSelf)
            _iceAura.gameObject.SetActive(true);
        _iceAura.SetFreezePower(value);
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
