using Lean.Touch;
using RimuruDev;
using UnityEngine;

namespace _Sources.Input
{
    public class SwipeReader : MonoBehaviour
    {
        private const float MinSwipeDistance = 50;

        [SerializeField] private InputReader _inputReader;
        [SerializeField] private DeviceTypeDetector _deviceTypeDetector;
    
        private int _step = 1;
        private bool _isInit;
        private bool _isMobile;

        private void OnEnable()
        {
            LeanTouch.OnFingerSwipe += HandleSwipe;
        }

        private void OnDisable()
        {
            LeanTouch.OnFingerSwipe -= HandleSwipe;
        }

        private void Start()
        {
            _isMobile = _deviceTypeDetector.CurrentDeviceType == CurrentDeviceType.WebMobile;
        }
    
        private void HandleSwipe(LeanFinger finger)
        {
            if(_isMobile == false)
                return;
            
            if (finger.StartedOverGui)
                return;

            if (finger.SwipeScreenDelta.magnitude >= MinSwipeDistance)
            {
                Vector2 swipeDirection = finger.SwipeScreenDelta.normalized;
                ProcessSwipe(swipeDirection);
            }
        }

        private void ProcessSwipe(Vector2 direction)
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x > 0)
                    _inputReader.OnMove(new GameMapVector2(0, _step));
                else
                    _inputReader.OnMove(new GameMapVector2(0, -_step));
            }
            else
            {
                if (direction.y > 0)
                    _inputReader.OnMove(new GameMapVector2(-_step, 0));
                else
                    _inputReader.OnMove(new GameMapVector2(_step, 0));
            }
        }
    }
}