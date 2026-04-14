using Lean.Touch;
using UnityEngine;

namespace _Sources.Input
{
    public class SwipeReader : MonoBehaviour
    {
        private const float MinSwipeDistance = 50;

        [SerializeField] private InputReader _inputReader;
    
        private int _step = 1;
        private bool _isInit;
        private bool _isActive;

        private void OnEnable()
        {
            LeanTouch.OnFingerSwipe += HandleSwipe;
        }

        private void OnDisable()
        {
            LeanTouch.OnFingerSwipe -= HandleSwipe;
            _isActive = false;
        }
    
        private void HandleSwipe(LeanFinger finger)
        {
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