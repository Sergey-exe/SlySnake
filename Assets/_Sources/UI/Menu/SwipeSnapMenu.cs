using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Sources.UI.Menu
{
    public class SwipeSnapMenu : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        private readonly List<float> _itemPositionsNormalized = new List<float>();

        [SerializeField] private RectTransform _contentContainer;
        [SerializeField] private Scrollbar _scrollBar;
        [SerializeField] private LevelMenu _levelMenu;
        [SerializeField] private float _snapSpeed;

        private float _targetScrollBarValueNormalized = 0;
        private float _itemSizeNormalized;

        private bool _isDragging;

        private Coroutine _snapRoutine;

        public event Action<int> TabSelected;
        public event Action<int> TabSnapped;

        public int SelectedTabIndex { get; private set; }

        private void OnEnable()
        {
            SelectedTab(_levelMenu.CurrentLevelIndex);
        }

        private void Start()
        {
            Recalculate();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDragging = true;
        
            if (_snapRoutine != null)
            {
                StopCoroutine(_snapRoutine);
                _snapRoutine = null;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _targetScrollBarValueNormalized = _scrollBar.value;

            _isDragging = false;

            FindSnappingTabAndStartSnapping();
        }
    
        public void SelectedTab(int tabIndex)
        {
            if (tabIndex < 0 || tabIndex >= _itemPositionsNormalized.Count)
                return;

            SelectedTabIndex = tabIndex;
            _targetScrollBarValueNormalized = _itemPositionsNormalized[tabIndex];

            StartSnap();

            TabSelected?.Invoke(tabIndex);
        }

        private void Recalculate()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_contentContainer);

            _itemPositionsNormalized.Clear();

            var itemsCount = _contentContainer.childCount;
            _itemSizeNormalized = 1f / (itemsCount - 1f);

            for (int i = 0; i < itemsCount; i++)
            {
                _itemPositionsNormalized.Add(_itemSizeNormalized * i);
            }

            SelectedTab(SelectedTabIndex);
        }

        private void FindSnappingTabAndStartSnapping()
        {
            for (int i = 0; i < _itemPositionsNormalized.Count; i++)
            {
                var itemPositionNormalized = _itemPositionsNormalized[i];

                if (_targetScrollBarValueNormalized < itemPositionNormalized + _itemSizeNormalized / 2f
                    && _targetScrollBarValueNormalized > itemPositionNormalized - _itemSizeNormalized / 2f)
                {
                    SelectedTab(i);
                    break;
                }
            }
        }

        private void StartSnap()
        {
            if (_snapRoutine != null)
            {
                StopCoroutine(_snapRoutine);
            }

            _snapRoutine = StartCoroutine(SnapRoutine());
        }

        private IEnumerator SnapRoutine()
        {
            if (_itemPositionsNormalized.Count < 2)
                yield break;

            float targetPosition = _itemPositionsNormalized[SelectedTabIndex];

            while (true)
            {
                _scrollBar.value = Mathf.Lerp(
                    _scrollBar.value,
                    targetPosition,
                    Time.deltaTime * _snapSpeed
                );

                if (Mathf.Abs(_scrollBar.value - targetPosition) <= 0.0001f)
                {
                    _scrollBar.value = targetPosition;
                    break;
                }

                yield return null;
            }

            _snapRoutine = null;
            TabSnapped?.Invoke(SelectedTabIndex);
        }
    }
}