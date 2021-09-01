using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    private readonly List<Enemy> _enemiesInZone = new List<Enemy>();
    
    private void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent(out Enemy enemy)) return;
        if (_enemiesInZone.Contains(enemy)) return;
        
        _enemiesInZone.Add(enemy);
    }

    public List<Enemy> GetEnemiesInZone()
    {
        var _diedEnemies = _enemiesInZone.Where(enemy => enemy.CurrentState == EnemyState.Died || enemy.CurrentState == EnemyState.Run).ToList();

        foreach (var diedEnemy in _diedEnemies)
        {
            _enemiesInZone.Remove(diedEnemy);
        }

        return _enemiesInZone;
    }
}
