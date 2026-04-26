using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private float _intensity;
    [SerializeField] private float _duration;
    
    private Coroutine _currentShake;

    public void Shake()
    {
        if (_currentShake != null)
            StopCoroutine(_currentShake);

        _currentShake = StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        float time = 0f;
        
        Vector3 originalPos = transform.localPosition;

        while (time < _duration)
        {
            Vector3 offset = Random.insideUnitSphere * _intensity;
            transform.localPosition = originalPos + offset;

            time += Time.deltaTime;
            yield return null;
        }
        
        transform.localPosition = originalPos;
        
        _currentShake = null;
    }
}