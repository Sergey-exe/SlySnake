using System;
using System.Collections;
using System.Collections.Generic;
using _Sources.Input;
using UnityEngine;

namespace _Sources.Player
{
    public class PlayersMover : MonoBehaviour, IMoveStopper
    {
        [SerializeField] private InputReader _inputReader;
        [SerializeField] private PlayersWayBuilder _wayBuilder;
        [SerializeField] private CameraShaker _cameraShaker;
        [SerializeField] private float _duration;
    
        private PlayersTransformData _playersTransformData;
        private readonly List<Coroutine> _moveCoroutines = new();
        private int _activeCoroutinesCount = 0;
        private bool _isMoving;
        private bool _isInit;

        public event Action<int, Transform> InPoint;
        public event Action PlayersFinished;
        public event Action PlayerFinished;

        private void OnEnable()
        {
            _inputReader.OnMove += TryStartMove;
        }
        
        private void OnDisable()
        {
            _inputReader.OnMove -= TryStartMove;
            
            StopActiveCoroutines();
        }
    
        public void Init(PlayersTransformData playersTransformData)
        {
            _playersTransformData = playersTransformData ?? throw new ArgumentNullException(nameof(playersTransformData));
            _isInit = true;
        }

        public void TryStartMove(GameMapVector2 direction)
        {
            if (!_isInit || _isMoving)
                return;

            StopActiveCoroutines();
            
            _isMoving = true;
            int count = _playersTransformData.PlayersCount;

            for (int i = 0; i < count; i++)
            {
                List<Transform> waypoints = _wayBuilder.SearchWay(i, direction);
                
                if (waypoints == null || waypoints.Count == 0) 
                    continue;

                _activeCoroutinesCount++;
                var coroutine = StartCoroutine(MovePlayer(i, _playersTransformData.GetTransform(i), waypoints));
                _moveCoroutines.Add(coroutine);
            }
            
            if (_activeCoroutinesCount == 0)
            {
                _isMoving = false;
                PlayersFinished?.Invoke();
            }
        }

        public void StopActiveCoroutines()
        {
            foreach (var coroutine in _moveCoroutines)
                if (coroutine != null) StopCoroutine(coroutine);
            
            _moveCoroutines.Clear();
            _activeCoroutinesCount = 0;
            _isMoving = false;
        }

        private IEnumerator MovePlayer(int mapIndex, Transform playerTransform, List<Transform> waypoints)
        {
            int waypointsCount = waypoints.Count;

            for (int i = 0; i < waypointsCount; i++)
            {
                Transform targetPoint = waypoints[i];
                Vector3 startPos = playerTransform.position;
                Vector3 endPos = targetPoint.position;
                float elapsedTime = 0f;
                
                while (elapsedTime < _duration)
                {
                    elapsedTime += Time.deltaTime;
                    float time = elapsedTime / _duration;
                    
                    playerTransform.position = Vector3.MoveTowards(startPos, endPos, time);
                    yield return null; 
                }

                playerTransform.position = endPos;
                InPoint?.Invoke(mapIndex, targetPoint);
            }

            OnPlayerReachedFinalPoint();
        }

        private void OnPlayerReachedFinalPoint()
        {
            _activeCoroutinesCount--;
            PlayerFinished?.Invoke();
            _cameraShaker.Shake();

            if (_activeCoroutinesCount <= 0)
            {
                _activeCoroutinesCount = 0;
                _isMoving = false;
                _moveCoroutines.Clear();
                PlayersFinished?.Invoke();
            }
        }
    }
}

public interface IMoveStopper
{
    void StopActiveCoroutines();
}
