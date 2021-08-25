using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Enemy> _templates;
    [SerializeField] private Player _player;
    [SerializeField] private int _limitOfEnemyCount;
    [SerializeField] private List<Vector3> _spawnPositions;
    [SerializeField] private float _spawnCooldown;

    private List<Enemy> _enemies = new List<Enemy>();
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
    
    private void SpawnEnemy()
    {
        if (_enemies.Count >= _limitOfEnemyCount) return;

        var enemy = Instantiate(_templates[Random.Range(0, _templates.Count)], _spawnPositions[Random.Range(0, _spawnPositions.Count)], Quaternion.identity,
            transform);

        enemy.Init(_player);
        
        _enemies.Add(enemy);
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
            Destroy(enemy);
        }
    }
}
