using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace _Sources.UI.Menu
{
    public class ScrollItemAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private RectTransform _visual;

        [SerializeField] private float _focusDuration = 0.2f;

        private Tween _focusTween;

        private float _currentFocus;

        public void OnEnable()
        {
            _animator.keepAnimatorStateOnDisable = true;
        }
        
        public void SetFocusTarget(float value)
        {
            value = Mathf.Clamp01(value);

            _focusTween?.Kill();

            _focusTween = DOTween.To(
                () => _currentFocus,
                x =>
                {
                    _currentFocus = x;

                    if (_animator != null)
                        _animator.SetFloat("focusAmount", _currentFocus);
                },
                value,
                _focusDuration
            ).SetEase(Ease.OutCubic);
        }
    }
}