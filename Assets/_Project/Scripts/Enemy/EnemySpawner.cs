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

    private int _currentWave = 0;
    private int _enemiesSpawned = 0;
    private float _timeSpend;
    private bool _inSession = false;

    public event UnityAction<Enemy> OnEnemyKilled;
    
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
            }

            _enemiesSpawned = 0;
        }

        var enemyType = GetEnemyType(_waves[_currentWave]);
        
        var enemy = Instantiate(_templates[enemyType], _spawnPositions[Random.Range(0, _spawnPositions.Count)].position, Quaternion.identity,
            transform);

        switch (enemyType)
        {
            case 0 :
                enemy.Init(_player, _waves[_currentWave].OneHanded);
                break;
            case 1 :
                enemy.Init(_player, _waves[_currentWave].TwoHanded);
                break;
            case 2 :
                enemy.Init(_player, _waves[_currentWave].Shield);
                break;
        }
        
        _enemies.Add(enemy);
        _enemiesSpawned++;
        
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
