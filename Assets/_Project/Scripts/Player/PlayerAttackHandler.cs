using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAttackHandler : MonoBehaviour
{
    private const float DefaultAttackCooldown = 1.25f;
    
    [SerializeField] private StonePeaks _stonePeaks;
    
    private Player _player;
    private Animator _animator;
    private DoubleDamage _doubleDamage;
    private List<Enemy> _enemiesInZone = new List<Enemy>();
    private float _currentAttackCooldown = DefaultAttackCooldown;
    private PlayerState _currentPlayerState;
    private float _timeToAttack;
    private bool _isPlayerAttack = false;

    public List<Enemy> EnemiesInZone => _enemiesInZone;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _doubleDamage = GetComponentInChildren<DoubleDamage>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            _enemiesInZone.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            _enemiesInZone.Remove(enemy);
        }
    }

    private void Update()
    {
        Attack();
    }

    private IEnumerator StonePeaksCoroutine()
    {
        _animator.SetTrigger("StonePeaks");
        _animator.speed = 1f;
        
        yield return new WaitForSeconds(0.75f);

        Instantiate(_stonePeaks, transform);
    }

    private void Attack()
    {
        if (_currentPlayerState != PlayerState.Attack)
        {
            _isPlayerAttack = false;
            return;
        }

        if (!_isPlayerAttack)
        {
            _timeToAttack = _currentAttackCooldown * 0.3f;
            _isPlayerAttack = true;
        }

        if (_timeToAttack >= _currentAttackCooldown)
        {
            var _diedEnemies = _enemiesInZone.Where(enemy => enemy.CurrentState == EnemyState.Died).ToList();

            foreach (var diedEnemy in _diedEnemies)
            {
                _enemiesInZone.Remove(diedEnemy);
            }
            
            foreach (var enemy in _enemiesInZone)
            {
                enemy.TakeDamage(_player.PlayerStats.Damage * (_doubleDamage.IsDoubleDamage ? 2 : 1));
            }

            _timeToAttack = 0f;
        }

        _timeToAttack += Time.deltaTime;
    }

    public void DecreaseCooldown(float value)
    {
        _currentAttackCooldown = DefaultAttackCooldown / value;
    }
    
    public void ToAttack()
    {
        _animator.speed = _player.PlayerStats.AttackSpeed;
        _animator.SetBool("IsAttack", true);
        _currentPlayerState = PlayerState.Attack;
    }

    public void ToIdle()
    {
        _animator.speed = 1;
        _animator.SetBool("IsAttack", false);
        _currentPlayerState = PlayerState.Idle;
    }
    
    public void UseStonePeaks()
    {
        StartCoroutine(StonePeaksCoroutine());
        _currentPlayerState = PlayerState.UseSkill;
    }
}

public enum PlayerState
{
    Idle,
    Attack,
    UseSkill
}
