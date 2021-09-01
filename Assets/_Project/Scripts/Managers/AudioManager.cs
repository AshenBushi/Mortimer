using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private List<AudioClip> _backgroundsMusic;
    [SerializeField] private List<AudioClip> _slashes;

    public List<AudioClip> Slashes => _slashes;

    protected override void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBattleMusic()
    {
        _musicSource.clip = _backgroundsMusic[1];
        _musicSource.Play();
    }
    
    public void PlayMenuMusic()
    {
        _musicSource.clip = _backgroundsMusic[0];
        _musicSource.Play();
    }
}
