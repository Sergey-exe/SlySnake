using RimuruDev;
using UnityEngine;

[RequireComponent(typeof(DeviceTypeDetector))]
public class PlatformVisibility : MonoBehaviour
{
    [SerializeField] private GameObject[] _objectsForPC;
    [SerializeField] private GameObject[] _objectsForMobile;
    
    public void Start()
    {
        bool isMobile = GetComponent<DeviceTypeDetector>().CurrentDeviceType == CurrentDeviceType.WebMobile;
        
        foreach (var gameObject in _objectsForMobile)
            gameObject.SetActive(isMobile);
        
        foreach (var gameObject in _objectsForPC)
            gameObject.SetActive(!isMobile);
    }
}
