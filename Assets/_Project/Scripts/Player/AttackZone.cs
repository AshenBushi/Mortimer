using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    private readonly List<Enemy> _enemiesInZone = new List<Enemy>();
    
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

    public List<Enemy> GetEnemiesInZone()
    {
        var _diedEnemies = _enemiesInZone.Where(enemy => enemy.CurrentState == EnemyState.Died).ToList();

        foreach (var diedEnemy in _diedEnemies)
        {
            _enemiesInZone.Remove(diedEnemy);
        }

        return _enemiesInZone;
    }
}
