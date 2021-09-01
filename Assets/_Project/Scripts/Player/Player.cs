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

    private PlayerAttackHandler _playerAttackHandler;
    private Animator _animator;
    
    public int Money { get; private set; }
    public PlayerStats PlayerStats => _playerStats;
    public event UnityAction OnHealthChanged;
    public event UnityAction OnMoneyChanged;

    private void Awake()
    {
        _playerAttackHandler = GetComponent<PlayerAttackHandler>();
        _ultimateDefense = GetComponentInChildren<UltimateDefense>();
        _animator = GetComponent<Animator>();
        SetMoney();
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
        DataManager.Instance.Data.Money = Money;
        DataManager.Instance.Save();
    }
    
    private void Die()
    {
        _animator.SetTrigger("Died");
        _playerAttackHandler.Die();
        SessionManager.Instance.EndSession();
    }
    
    private void OnEnemyKilled(Enemy enemy)
    {
        AddMoney((int)(enemy.MoneyReward * PerksHandler.Instance.GetPerkBoost(PerkName.Income)));
    }
    
    private void SetMoney()
    {
        Money = DataManager.Instance.Data.Money;
        OnMoneyChanged?.Invoke();
    }
    
    public void AddMoney(int value)
    {
        Money += value;
        OnMoneyChanged?.Invoke();
    }
    
    public void SpendMoney(int value)
    {
        Money -= value;
        OnMoneyChanged?.Invoke();
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
