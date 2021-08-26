using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    private const int DefaultMaxHealth = 200;
    private const int DefaultDamage = 10;
    
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private AttackSkill _attackSkill;
    [Space]
    [SerializeField] private PlayerStats _playerStats;

    private Animator _animator;
    
    public PlayerStats PlayerStats => _playerStats;
    
    public event UnityAction OnHealthChanged;
    public event UnityAction OnExperienceChanged;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Init()
    {
        _playerStats = new PlayerStats(DefaultMaxHealth * PerksHandler.Instance.GetPerkBoost(PerksName.Health),
            DefaultDamage * PerksHandler.Instance.GetPerkBoost(PerksName.Damage));
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
        
    }

    private void OnEnemyKilled(Enemy enemy)
    {
        User.Instance.AddMoney((int)(enemy.MoneyReward * PerksHandler.Instance.GetPerkBoost(PerksName.Income)));
    }

    private IEnumerator UseSkillCoroutine()
    {
        _animator.Play("Skill1");

        yield return new WaitForSeconds(0.66f);

        Instantiate(_attackSkill, transform);
    }

    public void TakeDamage(int damage)
    {
        _playerStats.Health -= damage;

        OnHealthChanged?.Invoke();

        if (_playerStats.Health <= 0)
        {
            Die();
        }
    }

    public void Attack()
    {
        _animator.Play("Attack1");
    }

    public void Idle()
    {
        _animator.Play("Idle");
    }

    public void UseSkill()
    {
        StartCoroutine(UseSkillCoroutine());
    }
}

[Serializable]
public struct PlayerStats
{
    public int MaxHealth;
    public int Health;
    public int Damage;

    public PlayerStats(float maxHealth, float damage)
    {
        MaxHealth = (int)maxHealth;
        Health = MaxHealth;
        Damage = (int)damage;
    }
}
