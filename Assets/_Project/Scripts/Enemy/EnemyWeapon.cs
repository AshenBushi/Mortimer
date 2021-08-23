using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.TakeDamage(_enemy.Damage);
        }
    }
}
