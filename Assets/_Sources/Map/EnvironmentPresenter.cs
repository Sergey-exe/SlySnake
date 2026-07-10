using _Sources.Map;
using UnityEngine;

public class EnvironmentPresenter : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private SoundChanger _soundChanger;
    [SerializeField] private EnvironmentSetsData _environmentSetsData;
    [SerializeField] private Transform _mapsCollector;

    public void Setup(EnvironmentSetsType backgroundType)
    {
        var config = _environmentSetsData.Environments[backgroundType];
        
        _camera.backgroundColor = config.BackgroundColor;
        _soundChanger.ChangeSound(config.BackgroundSound);

        foreach (var gameObject in config.SpecialObjects)
        {
            if (gameObject != null)
                Instantiate(gameObject, _mapsCollector);
        }
    }

    public void ResetToDefault()
    {
        Setup(EnvironmentSetsType.Default);
    }

    public AudioClip GetImpactSound(EnvironmentSetsType type)
    {
        return _environmentSetsData.Environments[type].ImpactSound;
    }
}