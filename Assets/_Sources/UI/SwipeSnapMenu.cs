using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Sources.UI
{
    public class SwipeSnapMenu : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        private readonly List<float> _itemPositionsNormalized = new List<float>();
    
        [SerializeField] private RectTransform _contentContainer;
        [SerializeField] private Scrollbar _scrollbar;
        [SerializeField] private float _snapSpeed = 15f;
    
        private bool _isDragging;
        private bool _isSnapping;
    
        private float _targetScrollBarValueNormalized = 0;
        private float _itemSizeNormalized;
    
        public event Action<int> TabSelected;
    
        [field: SerializeField] public int SelectedTabIndex { get; private set; }

        private void Update()
        {
            if (_isDragging)
                return;
        
            if(_isSnapping)
                SnapContent();
        }

        public void Start()
        {
            Recalculate();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDragging = true;
            _isSnapping = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _targetScrollBarValueNormalized = _scrollbar.value;
            _isDragging = false;
            _isSnapping = true;
        
            FindSnappingTabAndStartSnapping();
        }

        public void Recalculate()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_contentContainer);
        
            _itemPositionsNormalized.Clear();
        
            var itemsCount = _contentContainer.childCount;
            _itemSizeNormalized = 1f / (itemsCount - 1f);

            for (int i = 0; i < itemsCount; i++)
            {
                var itemPositionNormalized = _itemSizeNormalized * i;
                _itemPositionsNormalized.Add(itemPositionNormalized);
            }    
        
            SelectTab(SelectedTabIndex + 1);
        }

        public void SelectTab(int tabIndex)
        {
            if(tabIndex < 0 || tabIndex >= _itemPositionsNormalized.Count)
                return;
        
            SelectedTabIndex = tabIndex;
            _targetScrollBarValueNormalized = _itemPositionsNormalized[tabIndex];
            _isSnapping = true;
        
            TabSelected?.Invoke(tabIndex);
        }

        public void FindSnappingTabAndStartSnapping()
        {
            for (int i = 0; i < _itemPositionsNormalized.Count; i++)
            {
                var itemPositionNormalized = _itemPositionsNormalized[i];
            
                if (_targetScrollBarValueNormalized < itemPositionNormalized + _itemSizeNormalized / 2f
                    && _targetScrollBarValueNormalized > itemPositionNormalized - _itemSizeNormalized / 2f)
                {
                    SelectTab(i);
                    break;
                }
            }
        }
    
        public void SnapContent()
        {
            if (_itemPositionsNormalized.Count < 2)
            {
                _isSnapping = false;
                return;
            }
        
            var targetPosition = _itemPositionsNormalized[SelectedTabIndex];
            _scrollbar.value = Mathf.Lerp(_scrollbar.value, targetPosition, Time.deltaTime * _snapSpeed);

            if (Mathf.Abs(_scrollbar.value - targetPosition) <= 0.0001f)
            {
                _isSnapping = false;
                TabSelected?.Invoke(SelectedTabIndex);
            }
        }
    }
}
