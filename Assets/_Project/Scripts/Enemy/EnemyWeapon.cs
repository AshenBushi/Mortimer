using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Player player)) return;
        if (_enemy.CurrentState == EnemyState.Attack)
        {
            player.TakeDamage(_enemy.Damage);
        }
    }
}
