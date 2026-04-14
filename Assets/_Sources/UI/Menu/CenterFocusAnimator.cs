using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using _Sources.UI.Menu;

public class CenterFocusAnimator : MonoBehaviour
{
    [SerializeField] private RectTransform _viewport;
    [SerializeField] private ScrollRect _scrollRect;

    [SerializeField] private float _maxScale = 1.1f;
    [SerializeField] private float _minScale = 1f;
    [SerializeField] private float _maxDistance = 300f;

    private ScrollItemAnimator[] _items;
    private List<RectTransform> _rects;

    private void OnEnable()
    {
        _scrollRect.onValueChanged.AddListener(OnScroll);
    }

    private void OnDisable()
    {
        _scrollRect.onValueChanged.RemoveListener(OnScroll);
    }

    private void Start()
    {
        _items = GetComponentsInChildren<ScrollItemAnimator>();
        _rects = new List<RectTransform>(_items.Length);

        foreach (var item in _items)
        {
            if (item != null && item.TryGetComponent(out RectTransform rect))
                _rects.Add(rect);
        }

        Canvas.ForceUpdateCanvases();
        UpdateItems();
    }

    private void OnScroll(Vector2 _)
    {
        UpdateItems();
    }

    private void UpdateItems()
    {
        if (_viewport == null)
            return;

        Vector3 viewportCenter = _viewport.TransformPoint(_viewport.rect.center);

        for (int i = 0; i < _items.Length; i++)
        {
            var anim = _items[i];
            var item = _rects[i];

            if (anim == null || item == null)
                continue;

            Vector3 itemCenter = item.TransformPoint(item.rect.center);

            float distance = Mathf.Abs(itemCenter.x - viewportCenter.x);
            float normalizedDistance = Mathf.Clamp01(distance / _maxDistance);

            float focus = 1f - normalizedDistance;

            anim.SetFocusTarget(focus);
        }
    }
}