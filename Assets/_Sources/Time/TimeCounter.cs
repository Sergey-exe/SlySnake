using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace _Sources.Model
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
            
            if(_timeCoroutine == null)
                _timeCoroutine = StartCoroutine(TimeCoroutine());
        }

        public void StopCounting()
        {
            if (_isActive == false)
                return;
            
            if(_timeCoroutine != null)
                StopCoroutine(_timeCoroutine);
            
            _timeCoroutine = null;
            Debug.Log("StopCounting");
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