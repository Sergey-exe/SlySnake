using System;
using System.Collections;
using System.Collections.Generic;
using _Sources.Player;
using UnityEngine;

public class PlayerSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    
    public event Action<PlayerSoundPlayer> Destroy;

    private void OnDestroy()
    {
        Destroy?.Invoke(this);
    }

    public void SetClip(AudioClip clip)
    {
        _audioSource.clip = clip;
    }

    public void Play()
    {
        _audioSource.Play();
    }
}
