using System;
using System.Collections;
using _Sources.TimeManagement;
using TMPro;
using UnityEngine;

namespace _Sources.TimeManagement
{
    public class LevelTimeCounter : MonoBehaviour
    {
        [SerializeField] private LevelTimeViewer _levelTimeViewer;
        [SerializeField] private float _timeStep;
        
        private Coroutine _timeCoroutine;
        private WaitForSeconds _waitForSeconds;
        private bool _isInit;
        private bool _isActive;
    
        public float GameTime { get; private set; }

        public void Init()
        {
            _waitForSeconds = new WaitForSeconds(_timeStep);
            
            _isInit = true;
        }
    
        public void Activate()
        {
            if(!_isInit)
                return;

            _isActive = true;
        }

        public void StartCounting()
        {
            if (_isActive == false)
                return;
            
            if(_timeCoroutine != null)
                return;
            
            _timeCoroutine = StartCoroutine(TimeCoroutine());
            _levelTimeViewer.ShowTimers();
        }

        public void StopCounting()
        {
            if (_isActive == false)
                return;
            
            if(_timeCoroutine != null)
                StopCoroutine(_timeCoroutine);
            
            _timeCoroutine = null;
            _levelTimeViewer.HideTimers();
        }

        public void Revert()
        {
            GameTime = 0;
        }

        private IEnumerator TimeCoroutine()
        {
            WaitForSeconds wait = _waitForSeconds;
        
            while (_isActive)
            {
                GameTime += _timeStep;
                _levelTimeViewer.ShowTime(GameTime);
                
                yield return wait;
            }
        }
    }
}