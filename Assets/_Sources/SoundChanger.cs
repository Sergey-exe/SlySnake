using System.Collections;
using UnityEngine;

public class SoundChanger : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _fadeDuration = 1.0f;

    private Coroutine _fadeCoroutine;

    private void Start()
    {
        _audioSource.Play();
    }

    public void ChangeSound(AudioClip clip)
    {
        if (_audioSource.clip == clip && _audioSource.isPlaying)
            return;
        
        Debug.Log(_audioSource.clip + " != "  + clip);

        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(FadeTrack(clip));
    }

    private IEnumerator FadeTrack(AudioClip newClip)
    {
        float startVolume = _audioSource.volume;

        if (_audioSource.isPlaying)
        {
            while (_audioSource.volume > 0)
            {
                _audioSource.volume -= startVolume * Time.deltaTime / _fadeDuration;
                yield return null;
            }
        }

        _audioSource.Stop();
        _audioSource.clip = newClip;
        _audioSource.Play();

        while (_audioSource.volume < startVolume)
        {
            _audioSource.volume += startVolume * Time.deltaTime / _fadeDuration;
            yield return null;
        }

        _audioSource.volume = startVolume;
        _fadeCoroutine = null;
    }
}