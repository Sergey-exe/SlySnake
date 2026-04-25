using UnityEngine;

namespace _Sources.Map
{
    [CreateAssetMenu(fileName = "New EnvironmentSet", menuName = "EnvironmentSet/Create new EnvironmentSet", order = 51)]
    public class EnvironmentSet : ScriptableObject
    {
        [SerializeField] private Color _backgroundColor;
        [SerializeField] private AudioClip _backgroundSound;
        [SerializeField] private AudioClip _impactSound;
        [SerializeField] private GameObject[] _specialObjects;
        
        public Color BackgroundColor => _backgroundColor;
        public AudioClip BackgroundSound => _backgroundSound;
        public AudioClip ImpactSound => _impactSound;
        public GameObject[] SpecialObjects => _specialObjects;
    }
}
