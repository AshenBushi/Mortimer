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
    private Collider _collider;
    private EnemyState _currentState;
    private bool _isInit = false;

    public int Damage => _damage;
    public int MoneyReward => _moneyReward;
    public int ExperienceReward => _experienceReward;
    public EnemyState CurrentState => _currentState;

    public event UnityAction<Enemy> OnEnemyDied;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
    }

    public void Init(Player target)
    {
        _target = target;
        _isInit = true;
    }
    
    private void Update()
    {
        if (_currentState == EnemyState.Died || !_isInit) return;
        
        if (Vector3.Distance(_target.transform.position, transform.position) > 2.5f)
        {
            RunToTarget();
        }
        
        if(_currentState == EnemyState.Run)
            Run();
    }

    private void Run()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, Time.deltaTime * _speed);
        transform.LookAt(_target.transform);

        if (!(Vector3.Distance(_target.transform.position, transform.position) < 2.4)) return;
        if (_currentState == EnemyState.Died) return;
        AttackTarget();
    }

    private void RunToTarget()
    {
        _animator.Play("Run");
        _currentState = EnemyState.Run;
    }

    private void AttackTarget()
    {
        _animator.Play("Attack1");
        _currentState = EnemyState.Attack;
    }

    private IEnumerator Die()
    {
        if (_currentState == EnemyState.Died) yield break;
        
        _currentState = EnemyState.Died;
        gameObject.layer = LayerMask.NameToLayer("Dead");

        var dieAnimationIndex = Random.Range(1, 5);
        
        _animator.Play("Die" + dieAnimationIndex);
        
        OnEnemyDied?.Invoke(this);
        
        yield return new WaitForSeconds(1f);
        
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            StartCoroutine(Die());
        }
    }
}

public enum EnemyState
{
    Idle,
    Run,
    Attack,
    Died
}
