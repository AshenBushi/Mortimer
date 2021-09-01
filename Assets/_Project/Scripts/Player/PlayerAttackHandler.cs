using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAttackHandler : MonoBehaviour
{
    private const float DefaultAttackSpeed = 1.25f;

    [SerializeField] private AttackZone _attackZone;
    [SerializeField] private StonePeaks _stonePeaks;
    [SerializeField] private List<float> _timingForAttacks;
    [SerializeField] private List<float> _attacksAnimationDurations;
    [SerializeField] private float _timingForStonePeaks;
    [SerializeField] private float _stonePeaksAnimationDuration;
    
    private Player _player;
    private Animator _animator;
    private DoubleDamage _doubleDamage;
    private PlayerState _currentPlayerState;
    private AudioSource _audioSource;
    private int _currentAttack;
    private bool _isPlayerAttack;
    private bool _isPlayerUseSkill;

    private float AttackSpeed => _player.PlayerStats.AttackSpeed;
    private List<AudioClip> _slashes => AudioManager.Instance.Slashes;

    private void Awake()
    {
        _player = GetComponent<Player>();
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
        if(_currentPlayerState == PlayerState.Die) return;

        if (_isPlayerAttack) return;
        if (_isPlayerUseSkill) return;

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
            case PlayerState.UseSkill:
                _animator.Play("StonePeaks");
                _animator.speed = 1f;
                StartCoroutine(StonePeaksCoroutine());
                break;
        }
    }
    
    private IEnumerator StonePeaksCoroutine()
    {
        _isPlayerUseSkill = true;

        yield return new WaitForSeconds(_timingForStonePeaks);

        Instantiate(_stonePeaks, _stonePeaks.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(_stonePeaksAnimationDuration - _timingForStonePeaks);

        _currentPlayerState = PlayerState.Idle;
        _isPlayerUseSkill = false;
    }

    private IEnumerator AttackCoroutine()
    {
        _isPlayerAttack = true;
        
        yield return new WaitForSeconds(_timingForAttacks[_currentAttack] / AttackSpeed);
        
        Attack();

        yield return new WaitForSeconds((_attacksAnimationDurations[_currentAttack] - _timingForAttacks[_currentAttack]) / AttackSpeed);
        
        if (_currentAttack == _timingForAttacks.Count - 1)
            _currentAttack = 0;
        else
            _currentAttack++;

        _isPlayerAttack = false;
    }

    private void Attack()
    {
        var enemiesInZone = _attackZone.GetEnemiesInZone();
            
        if(enemiesInZone.Count > 0)
            PlaySlashSound();
        
        foreach (var enemy in enemiesInZone)
        {
            enemy.TakeDamage(_player.PlayerStats.Damage * (_doubleDamage.IsDoubleDamage ? 2 : 1));
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

    public void ToIdle()
    {
        _currentPlayerState = PlayerState.Idle;
    }

    public void Die()
    {
        _currentPlayerState = PlayerState.Die;
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
    UseSkill,
    Die
}
