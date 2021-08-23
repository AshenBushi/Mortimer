using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private Player _player;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(_player.Damage);
        }
    }
}
