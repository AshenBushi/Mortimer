using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
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

    private PlayerStateMachine _playerStateMachine;
    private Stamina _stamina;
    private Animator _animator;
    private float _timer;
    
    public PlayerStats PlayerStats => _playerStats;
    public event UnityAction OnHealthChanged;

    private void Awake()
    {
        _playerStateMachine = GetComponent<PlayerStateMachine>();
        _stamina = GetComponent<Stamina>();
        _ultimateDefense = GetComponentInChildren<UltimateDefense>();
        _animator = GetComponent<Animator>();
    }
    
    public void Init()
    {
        _playerStats = new PlayerStats(DefaultMaxHealth * PerksHandler.Instance.GetPerkBoost(PerkName.Health),
            DefaultDamage * PerksHandler.Instance.GetPerkBoost(PerkName.Damage), PerksHandler.Instance.GetPerkBoost(PerkName.Dodge));
        
        _stamina.Init();
        
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

    private void Update()
    {
        _timer += Time.deltaTime;

        if (!(_timer >= 1)) return;
        
        if (_playerStats.Health < _playerStats.MaxHealth * 0.35f)
        {
            _playerStats.Health += _playerStats.RegenerationPerSec;
        }

        _timer = 0;
    }

    private void Die()
    {
        _animator.SetTrigger("Died");
        _playerStateMachine.Die();
        SessionManager.Instance.EndSession();
    }
    
    private void OnEnemyKilled(Enemy enemy)
    {
        Wallet.Instance.AddMoney((int)(enemy.MoneyReward * PerksHandler.Instance.GetPerkBoost(PerkName.Income)));
    }

    public void TakeDamage(int damage)
    {
        if(_ultimateDefense.IsUltimateShield) return;

        if (_playerStateMachine.IsBlocking)
        {
            _stamina.SpendStamina(30);
            return;
        }

        if (_playerStats.DodgeChance > 0)
        {
            var randomValue = Random.Range(1, 101);
            
            if(randomValue <= _playerStats.DodgeChance) return;
        }

        var damageResist = _playerStats.Health < _playerStats.MaxHealth * 0.20f ? _playerStats.DamageResist : 0;
        
        _playerStats.Health -= damage * (1 - damageResist);

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
        _playerStats.DodgeChance = (int)PerksHandler.Instance.GetPerkBoost(PerkName.Dodge) + value;
    }
    
    public void UpgradeStonePeaks(int value)
    {
        if(!_activeSkills[0].IsActive)
            _activeSkills[0].Enable();
        
        _activeSkills[0].SetCooldown(value);
    }

    public void UpgradeIronWill(int value)
    {
        _playerStats.RegenerationPerSec = value;
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

    public void UpgradeRage(int value)
    {
        _playerStats.AdditionalDamage = value;
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

    public void UpgradeFortitude(int value)
    {
        _playerStats.DamageResist = value;
    }
}

[Serializable]
public struct PlayerStats
{
    public int MaxHealth;
    public int Health;
    public int Damage;
    public int AdditionalDamage;
    public float AttackSpeed;
    public int DodgeChance;
    public int RegenerationPerSec;
    public int DamageResist;

    public PlayerStats(float maxHealth, float damage, float dodgeChance)
    {
        MaxHealth = (int)maxHealth;
        Health = MaxHealth;
        Damage = (int)damage;
        AdditionalDamage = 0;
        AttackSpeed = 1;
        DodgeChance = (int)dodgeChance;
        RegenerationPerSec = 1;
        DamageResist = 0;
    }
}
