using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Sources.Player
{
    public class PlayersMover : MonoBehaviour
    {
        [SerializeField] private PlayersWayBuilder _wayBuilder;
        [SerializeField] private float _duration;
    
        private PlayersTransformData _playersTransformData;
        private readonly List<Coroutine> _moveCoroutines = new();
        private int _activeCoroutinesCount = 0;
        private bool _isMoving;
        private bool _isInit;

        public event Action<int, Transform> InPoint;
        public event Action PlayersFinished;
        public event Action PlayerFinished;
    
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
                
                // Если пути нет, сразу уменьшаем счетчик потенциальных корутин
                if (waypoints == null || waypoints.Count == 0) continue;

                _activeCoroutinesCount++;
                var coroutine = StartCoroutine(MovePlayerRoutine(i, _playersTransformData.GetTransform(i), waypoints));
                _moveCoroutines.Add(coroutine);
            }

            // Если ни один игрок не смог начать движение
            if (_activeCoroutinesCount == 0)
            {
                _isMoving = false;
                PlayersFinished?.Invoke();
            }
        }

        private void StopActiveCoroutines()
        {
            foreach (var coroutine in _moveCoroutines)
            {
                if (coroutine != null) StopCoroutine(coroutine);
            }
            _moveCoroutines.Clear();
            _activeCoroutinesCount = 0;
        }

        private IEnumerator MovePlayerRoutine(int mapIndex, Transform playerTransform, List<Transform> waypoints)
        {
            // Кэшируем количество точек
            int waypointsCount = waypoints.Count;

            for (int i = 0; i < waypointsCount; i++)
            {
                Transform targetPoint = waypoints[i];
                Vector3 startPos = playerTransform.position;
                Vector3 endPos = targetPoint.position;
                float elapsedTime = 0f;

                // Движение к конкретной точке
                while (elapsedTime < _duration)
                {
                    elapsedTime += Time.deltaTime;
                    float t = elapsedTime / _duration;
                    
                    // Используем SmoothStep для более приятного движения (опционально)
                    playerTransform.position = Vector3.Lerp(startPos, endPos, t);
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

            if (_activeCoroutinesCount <= 0)
            {
                _activeCoroutinesCount = 0;
                _isMoving = false;
                _moveCoroutines.Clear();
                PlayersFinished?.Invoke();
            }
        }
        
        private void OnDisable() => StopActiveCoroutines();
    }
}
