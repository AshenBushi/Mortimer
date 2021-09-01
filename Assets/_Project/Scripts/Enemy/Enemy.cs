using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;
    [SerializeField] private int _moneyReward;
    [SerializeField] private int _experienceReward;
    
    private Player _target;
    private Animator _animator;
    private EnemyState _currentState;
    private bool _isInit = false;
    private float _attackSpeed;
    private AudioSource _audioSource;
    
    public int Damage => _damage;
    public int MoneyReward => _moneyReward;
    public int ExperienceReward => _experienceReward;
    public EnemyState CurrentState => _currentState;

    public event UnityAction<Enemy> OnEnemyDied;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void Init(Player target, EnemyStats stats)
    {
        _currentState = EnemyState.Idle;
        gameObject.SetActive(true);
        _target = target;
        _health = stats.Health;
        _damage = stats.Damage;
        _moneyReward = stats.MoneyReward;
        _experienceReward = stats.Experience;
        _isInit = true;
        _attackSpeed = 1f;
    }
    
    private void Update()
    {
        if (_currentState == EnemyState.Died || !_isInit) return;
        
        transform.LookAt(_target.transform);
        
        if (Vector3.Distance(_target.transform.position, transform.position) > 2.45f)
        {
            RunToTarget();
        }
        
        if(_currentState == EnemyState.Run)
            Run();
    }

    private void Run()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, Time.deltaTime * _speed);

        if (Vector3.Distance(_target.transform.position, transform.position) > 2.4) return;
        if (_currentState == EnemyState.Died) return;
        AttackTarget();
    }

    private void RunToTarget()
    {
        _animator.speed = 1;
        _animator.Play("Run");
        _currentState = EnemyState.Run;
    }

    private void AttackTarget()
    {
        _animator.speed = _attackSpeed;
        _animator.Play("Attack1");
        _currentState = EnemyState.Attack;
    }

    private IEnumerator Die()
    {
        if (_currentState == EnemyState.Died) yield break;
        
        var dieAnimationIndex = Random.Range(1, 5);

        _animator.speed = 1;
        _animator.Play("Die" + dieAnimationIndex);
        
        _currentState = EnemyState.Died;
        gameObject.layer = LayerMask.NameToLayer("Dead");

        OnEnemyDied?.Invoke(this);
        
        yield return new WaitForSeconds(1f);
        
        gameObject.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    public void SetAttackSpeed(float value)
    {
        _animator.speed  = value;
    }

    public void Celebrate()
    {
        _isInit = false;
        _animator.Play("Idle");
    }
}

[Serializable]
public struct EnemyStats
{
    public int Chance;
    public int Experience;
    public int MoneyReward;
    public int Damage;
    public int Health;
}

public enum EnemyState
{
    Idle,
    Run,
    Attack,
    Died
}
