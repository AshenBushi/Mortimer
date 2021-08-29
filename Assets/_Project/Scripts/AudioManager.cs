using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private List<AudioClip> _backgroundsMusic;
    [SerializeField] private List<AudioClip> _slashes;
    [SerializeField] private AudioClip _footsteps;

    public List<AudioClip> Slashes => _slashes;
    public AudioClip Footsteps => _footsteps;

    private void Start()
    {
        TurnOnMenuMusic();
    }

    public void TurnOnBattleMusic()
    {
        _musicSource.clip = _backgroundsMusic[1];
        _musicSource.Play();
    }
    
    public void TurnOnMenuMusic()
    {
        _musicSource.clip = _backgroundsMusic[0];
        _musicSource.Play();
    }
}
