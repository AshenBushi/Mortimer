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
    
    private Player _target;
    private Animator _animator;
    private Collider _collider;
    private bool _isRunning = false;
    private bool _isDied = false;

    public int Damage => _damage;
    public int MoneyReward => _moneyReward;

    public event UnityAction<Enemy> OnEnemyDied;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
        _target = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (_isDied) return;
        
        if (Vector3.Distance(_target.transform.position, transform.position) > 2.4f)
        {
            RunToTarget();
        }
        
        if(_isRunning)
            Run();
    }

    private void Start()
    {
        RunToTarget();
    }

    private void Run()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, Time.deltaTime * _speed);
        transform.LookAt(_target.transform);

        if (!(Vector3.Distance(_target.transform.position, transform.position) < 2.4)) return;
        if (_isDied) return;
        AttackTarget();
    }

    private void RunToTarget()
    {
        _animator.Play("Run");
        _isRunning = true;
    }

    private void AttackTarget()
    {
        _animator.Play("Attack1");
        _isRunning = false;
    }

    private IEnumerator Die()
    {
        if (_isDied) yield break;
        
        _isDied = true;
        _isRunning = false;
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
