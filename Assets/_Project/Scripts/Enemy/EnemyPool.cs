using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private int _capacity;

    private List<Enemy> _oneHanded = new List<Enemy>();
    private List<Enemy> _twoHanded = new List<Enemy>();
    private List<Enemy> _shield = new List<Enemy>();

    public void Initialize(Enemy oneHandedPrefab, Enemy twoHandedPrefab, Enemy shieldPrefab)
    {
        for (var i = 0; i < _capacity; i++)
        {
            var spawned = Instantiate(oneHandedPrefab, transform);
            spawned.gameObject.SetActive(false);
            _oneHanded.Add(spawned);
        }
        
        for (var i = 0; i < _capacity; i++)
        {
            var spawned = Instantiate(twoHandedPrefab, transform);
            spawned.gameObject.SetActive(false);
            _twoHanded.Add(spawned);
        }
        
        for (var i = 0; i < _capacity; i++)
        {
            var spawned = Instantiate(shieldPrefab, transform);
            spawned.gameObject.SetActive(false);
            _shield.Add(spawned);
        }
    }

    public Enemy TryGetOneHanded()
    {
        var result = _oneHanded.FirstOrDefault(p => p.gameObject.activeSelf == false);

        return result;
    }
    
    public Enemy TryGetTwoHanded()
    {
        var result = _twoHanded.FirstOrDefault(p => p.gameObject.activeSelf == false);

        return result;
    }
    
    public Enemy TryGetShield()
    {
        var result = _shield.FirstOrDefault(p => p.gameObject.activeSelf == false);

        return result;
    }
}
