using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Aura : MonoBehaviour
{
    protected readonly List<Enemy> EnemiesInZone = new List<Enemy>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            EnemiesInZone.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            EnemiesInZone.Remove(enemy);
        }
    }

    protected void CheckEnemyList()
    {
        var _diedEnemies = EnemiesInZone.Where(enemy => enemy.CurrentState == EnemyState.Died).ToList();

        foreach (var diedEnemy in _diedEnemies)
        {
            EnemiesInZone.Remove(diedEnemy);
        }
    }
}
