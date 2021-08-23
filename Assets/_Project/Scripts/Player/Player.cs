using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _damage;
    [SerializeField] private AttackSkill _attackSkill;

    private Animator _animator;

    public int MaxHealth => _maxHealth;
    public int Damage => _damage;
    public int Health { get; private set; }
    public int Money { get; private set; } = 0;

    public event UnityAction OnHealthChanged;
    public event UnityAction OnMoneyChanged;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _enemySpawner.OnEnemyKilled += AddMoney;
    }
    
    private void OnDisable()
    {
        _enemySpawner.OnEnemyKilled -= AddMoney;
    }

    private void Start()
    {
        Health = _maxHealth;
        OnHealthChanged?.Invoke();
    }

    private void Die()
    {
        
    }

    private void AddMoney(Enemy enemy)
    {
        Money += enemy.MoneyReward;
        
        OnMoneyChanged?.Invoke();
    }

    private IEnumerator UseSkillCoroutine()
    {
        _animator.Play("Skill1");

        yield return new WaitForSeconds(0.66f);

        Instantiate(_attackSkill, transform);
    }
    
    public void TakeDamage(int damage)
    {
        Health -= damage;

        OnHealthChanged?.Invoke();

        if (Health <= 0)
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

    public void SpendMoney(int value)
    {
        Money -= value;
    }
}
