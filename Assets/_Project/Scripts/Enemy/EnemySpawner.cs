using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Enemy> _templates;
    [SerializeField] private Player _player;
    [SerializeField] private int _limitOfEnemyCount;
    [SerializeField] private List<Transform> _spawnPositions;
    [SerializeField] private float _spawnCooldown;
    [Space]
    [SerializeField] private List<Wave> _waves;

    private List<Enemy> _enemies = new List<Enemy>();

    private EnemyPool _enemyPool;
    private int _currentWave = 0;
    private int _enemiesSpawned = 0;
    private float _timeSpend;
    private bool _inSession = false;

    public int CurrentWave => _currentWave;

    public event UnityAction<Enemy> OnEnemyKilled;
    public event UnityAction<int, int> OnEnemySpawned;

    private void Awake()
    {
        _enemyPool = GetComponent<EnemyPool>();
    }

    private void Start()
    {
        _enemyPool.Initialize(_templates[0], _templates[1], _templates[2]);
    }

    private void Update()
    {
        if (!_inSession) return;
        
        _timeSpend += Time.deltaTime;

        if (!(_timeSpend >= _spawnCooldown)) return;
        SpawnEnemy();
        _timeSpend = 0;
    }

    private void OnEnemyDied(Enemy enemy)
    {
        enemy.OnEnemyDied -= OnEnemyDied;

        _enemies.Remove(enemy);
        
        OnEnemyKilled?.Invoke(enemy);
    }

    private int GetEnemyType(Wave wave)
    {
        var randomValue = Random.Range(1, 101);

        if (randomValue <= wave.OneHanded.Chance)
        {
            return 0;
        }
        
        return randomValue <= wave.OneHanded.Chance + wave.TwoHanded.Chance ? 1 : 2;
    }
    
    private void SpawnEnemy()
    {
        if (_enemies.Count >= _limitOfEnemyCount) return;

        if (_enemiesSpawned == 20)
        {
            if (_currentWave != _waves.Count - 1)
            {
                _currentWave++;
                _enemiesSpawned = 0;
            }
            else
            {
                _enemiesSpawned++;
            }
        }

        var enemyType = GetEnemyType(_waves[_currentWave]);

        Enemy enemy;
        
        switch (enemyType)
        {
            case 0:
                enemy = _enemyPool.TryGetOneHanded();
                break;
            case 1:
                enemy = _enemyPool.TryGetTwoHanded();
                break;
            case 2:
                enemy = _enemyPool.TryGetShield();
                break;
            default:
                enemy = _enemyPool.TryGetOneHanded();
                break;
        }

        enemy.transform.position = _spawnPositions[Random.Range(0, _spawnPositions.Count)].position;
        
        var enemyStats = enemyType switch
        {
            0 => _waves[_currentWave].OneHanded,
            1 => _waves[_currentWave].TwoHanded,
            2 => _waves[_currentWave].Shield,
            _ => new EnemyStats()
        };

        enemyStats.Health *= 1 + _enemiesSpawned / 100;
        enemyStats.Damage *= 1 + _enemiesSpawned / 100;
        enemyStats.Experience *= 1 + _enemiesSpawned / 100;
        enemyStats.MoneyReward *= 1 + _enemiesSpawned / 100;
        enemy.Init(_player, enemyStats);
        
        _enemies.Add(enemy);
        _enemiesSpawned++;
        
        OnEnemySpawned?.Invoke(_enemiesSpawned, _currentWave);
        
        enemy.OnEnemyDied += OnEnemyDied;
    }

    public void StartSession()
    {
        _inSession = true;
    }

    public void EndSession()
    {
        _inSession = false;
        
        foreach (var enemy in _enemies)
        {
            enemy.Celebrate();
        }
    }
}

[Serializable]
public class Wave
{
    public EnemyStats OneHanded;
    public EnemyStats TwoHanded;
    public EnemyStats Shield;
}
