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
    [SerializeField] private EnemyBar _enemyBar;
    
    private Player _target;
    private Animator _animator;
    private EnemyState _currentState;
    private bool _isInit;
    private float _attackSpeed;
    private bool _isStunning;
    
    public int Damage => _damage;
    public int MoneyReward => _moneyReward;
    public int ExperienceReward => _experienceReward;
    public EnemyState CurrentState => _currentState;

    public event UnityAction<Enemy> OnEnemyDied;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
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
        
        _enemyBar.gameObject.SetActive(true);
        _enemyBar.SetValue(_health, _health);
    }
    
    private void Update()
    {
        if(_isStunning) return;
        
        SelectState();
        CheckState();
    }

    private void SelectState()
    {
        if (!_isInit) return;
        
        transform.LookAt(_target.transform);
        
        if (Vector3.Distance(_target.transform.position, transform.position) > 2.45f)
            ToRunState();
        else
            ToAttackState();
    }
    
    private void CheckState()
    {
        switch (_currentState)
        {
            case EnemyState.Idle:
                _animator.Play("Idle");
                break;
            case EnemyState.Run:
                _animator.speed = 1;
                _animator.Play("Run");
                Run();
                break;
            case EnemyState.Attack:
                _animator.speed = _attackSpeed;
                _animator.Play("Attack1");
                break;
            case EnemyState.Died:
                return;
        }
    }

    private void Run()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, Time.deltaTime * _speed);
    }

    private void ToRunState()
    {
        _currentState = EnemyState.Run;
    }

    private void ToAttackState()
    {
        _currentState = EnemyState.Attack;
    }

    private IEnumerator Die()
    {
        _isInit = false;
        var dieAnimationIndex = Random.Range(1, 5);

        _animator.speed = 1;
        _animator.Play("Die" + dieAnimationIndex);
        
        _currentState = EnemyState.Died;
        gameObject.layer = LayerMask.NameToLayer("Dead");

        OnEnemyDied?.Invoke(this);
        
        yield return new WaitForSeconds(1f);
        
        gameObject.SetActive(false);
    }

    public void Celebrate()
    {
        _isInit = false;
        _currentState = EnemyState.Idle;
    }
    
    public void TakeDamage(int damage)
    {
        _health -= damage;
        
        _enemyBar.ChangeValue(_health);

        if (_health > 0) return;
        
        _enemyBar.gameObject.SetActive(false);
        StartCoroutine(Die());
    }
    
    public IEnumerator TakeDamageWithStunLock(int damage)
    {
        _health -= damage;
        
        _enemyBar.ChangeValue(_health);
        
        if (_health > 0)
        {
            _animator.Play("TakeDamage");

            _isStunning = true;

            yield return new WaitForSeconds(0.5f);

            _isStunning = false;
        }
        else
        {
            _enemyBar.gameObject.SetActive(false);
            
            StartCoroutine(Die());
        }
    }
    
    public void SetAttackSpeed(float value)
    {
        _animator.speed  = value;
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
