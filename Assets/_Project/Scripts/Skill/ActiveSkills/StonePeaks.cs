using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StonePeaks : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _delayToApplyDamage;

    private ParticleSystem _particle;
    private AudioSource _audioSource;
    private List<Enemy> _enemiesInZone = new List<Enemy>();

    private void Awake()
    {
        _particle = GetComponentInChildren<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(ApplyDamage());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            _enemiesInZone.Add(enemy);
        }
    }

    private IEnumerator ApplyDamage()
    {
        yield return new WaitForSeconds(_delayToApplyDamage);
        
        _audioSource.Play();

        foreach (var enemy in _enemiesInZone)
        {
            enemy.TakeDamage(_damage);
        }

        yield return new WaitUntil(() => _particle.isStopped);
            
        Destroy(gameObject);
    }
}
