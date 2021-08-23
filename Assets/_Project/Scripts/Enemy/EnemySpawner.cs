using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy _template;
    [SerializeField] private int _limitOfEnemyCount;
    [SerializeField] private List<Vector3> _spawnPositions;
    [SerializeField] private float _spawnCooldown;

    private List<Enemy> _enemies = new List<Enemy>();
    private float _timeSpend;

    public event UnityAction<Enemy> OnEnemyKilled;
    
    private void Start()
    {
        _timeSpend = _spawnCooldown;
    }

    private void Update()
    {
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
    
    private void SpawnEnemy()
    {
        if (_enemies.Count >= _limitOfEnemyCount) return;
        
        var enemy = Instantiate(_template, _spawnPositions[Random.Range(0, _spawnPositions.Count)], Quaternion.identity,
            transform);
        
        _enemies.Add(enemy);
        enemy.OnEnemyDied += OnEnemyDied;
    }
}
