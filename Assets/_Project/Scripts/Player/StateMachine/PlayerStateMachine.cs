using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private AttackZone _attackZone;
    [SerializeField] private StonePeaks _stonePeaks;
    [SerializeField] private List<float> _timingForAttacks;
    [SerializeField] private List<float> _attacksAnimationDurations;
    [SerializeField] private float _timingForStonePeaks;
    [SerializeField] private float _stonePeaksAnimationDuration;
    
    private Player _player;
    private Stamina _stamina;
    private Animator _animator;
    private DoubleDamage _doubleDamage;
    private PlayerState _currentPlayerState;
    private AudioSource _audioSource;
    private int _currentAttack;
    private bool _isPlayerAttacking;
    private bool _isPlayerUsingSkill;
    private bool _isPlayerBlocking;
    private bool _isPlayerDied = false;

    private float AttackSpeed => _player.PlayerStats.AttackSpeed;
    private List<AudioClip> _slashes => AudioManager.Instance.Slashes;

    public bool IsBlocking => _isPlayerBlocking;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _stamina = GetComponent<Stamina>();
        _animator = GetComponent<Animator>();
        _doubleDamage = GetComponentInChildren<DoubleDamage>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        CheckState();
    }

    private void CheckState()
    {
        if(_isPlayerDied || _isPlayerAttacking || _isPlayerBlocking || _isPlayerUsingSkill) return;

        switch (_currentPlayerState)
        {
            case PlayerState.Idle:
                _animator.Play("Idle");
                _animator.speed = 1;
                _currentAttack = 0;
                break;
            case PlayerState.Attack:
                _animator.speed = _player.PlayerStats.AttackSpeed;
                _animator.Play($"Attack{_currentAttack + 1}");
                StartCoroutine(AttackCoroutine());
                break;
            case PlayerState.Block:
                Block();
                break;
            case PlayerState.UseSkill:
                _animator.Play("StonePeaks");
                _animator.speed = 1f;
                StartCoroutine(StonePeaksCoroutine());
                break;
        }
    }
    
    private IEnumerator StonePeaksCoroutine()
    {
        _isPlayerUsingSkill = true;

        yield return new WaitForSeconds(_timingForStonePeaks);

        Instantiate(_stonePeaks, _stonePeaks.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(_stonePeaksAnimationDuration - _timingForStonePeaks);

        _currentPlayerState = PlayerState.Idle;
        _isPlayerUsingSkill = false;
    }

    private IEnumerator AttackCoroutine()
    {
        _isPlayerAttacking = true;
        
        yield return new WaitForSeconds(_timingForAttacks[_currentAttack] / AttackSpeed);
        
        Attack();

        yield return new WaitForSeconds((_attacksAnimationDurations[_currentAttack] - _timingForAttacks[_currentAttack]) / AttackSpeed);
        
        if (_currentAttack == _timingForAttacks.Count - 1)
            _currentAttack = 0;
        else
            _currentAttack++;

        _isPlayerAttacking = false;
    }
    
    private void Block()
    {
        _isPlayerBlocking = true;
        
        _animator.Play("Block");
        _animator.speed = 1;
    }

    private void Attack()
    {
        var enemiesInZone = _attackZone.GetEnemiesInZone();
            
        if(enemiesInZone.Count > 0)
            PlaySlashSound();

        var additionalDamage = _player.PlayerStats.Health < _player.PlayerStats.MaxHealth * 0.20f
            ? _player.PlayerStats.AdditionalDamage
            : 0;
        
        foreach (var enemy in enemiesInZone)
        {
            StartCoroutine(enemy.TakeDamageWithStunLock((_player.PlayerStats.Damage + additionalDamage) * (_doubleDamage.IsDoubleDamage ? 2 : 1)));
        }
    }
    
    private void PlaySlashSound()
    {
        _audioSource.pitch = Random.Range(0.9f, 1.1f);
        _audioSource.PlayOneShot(_slashes[Random.Range(0, _slashes.Count)]);
    }

    public void ToAttack()
    {
        _currentPlayerState = PlayerState.Attack;
    }

    public void ToBlock()
    {
        if(_stamina.StaminaCount <= 0) return;
        _currentPlayerState = PlayerState.Block;
    }

    public void ToIdle()
    {
        _currentPlayerState = PlayerState.Idle;
        _isPlayerBlocking = false;
    }

    public void Die()
    {
        _isPlayerDied = true;
    }
    
    public void UseStonePeaks()
    {
        _currentPlayerState = PlayerState.UseSkill;
    }
}

public enum PlayerState
{
    Idle,
    Attack,
    Block,
    UseSkill,
    Die
}
