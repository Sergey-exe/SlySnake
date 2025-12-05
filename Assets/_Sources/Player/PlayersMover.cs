using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersMover : MonoBehaviour
{
    [SerializeField] private PlayerWayBuilder _wayBuilder;
    [SerializeField] private float _duration;
    
    private PlayersTransformData _playersTransformData;
    private List<Coroutine> _moveCoroutines = new();
    private bool _isMoving;
    
    private bool _isInit;

    public event Action<int, Transform> InPoint;
        
    public void Init(PlayersTransformData playersTransformData)
    {
        _playersTransformData = playersTransformData ?? throw new ArgumentNullException(nameof(playersTransformData));
        
        _isInit = true;
    }

    public void TryStartMove(GameMapVector2 direction)
    {
        if(!_isInit)
            return;
        
        if (_isMoving)
            return;
        
        foreach (var coroutine in _moveCoroutines)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
        }
        
        _moveCoroutines.Clear();

        _isMoving = true;

        for (int i = 0; i < _playersTransformData.PlayersCount; i++)
        {
            List<Transform> waypoints = _wayBuilder.SearchWay(i, direction);
            
            var coroutine = StartCoroutine(MovePlayer(i, _playersTransformData.GetTransform(i) , waypoints, _duration));
            
            _moveCoroutines.Add(coroutine);
        }
        
        StartCoroutine(WaitForMovementEnd());
    }

    private IEnumerator WaitForMovementEnd()
    {
        foreach (var corutine in _moveCoroutines)
        {
            if (corutine != null)
                yield return corutine;
        }

        _isMoving = false;
    }

    private IEnumerator MovePlayer(int mapIndex, Transform playerTransform, List<Transform> waypoints, float duration)
    {
        if (waypoints == null || waypoints.Count == 0)
            yield break;

        int currentWaypointIndex = 0;

        while (currentWaypointIndex < waypoints.Count)
        {
            Transform startPoint = playerTransform;
            Transform targetPoint = waypoints[currentWaypointIndex];

            float elapsedTime = 0f;
            Vector3 startPos = startPoint.position;
            Vector3 endPos = targetPoint.position;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                playerTransform.position = Vector3.Lerp(startPos, endPos, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            playerTransform.position = endPos;
            InPoint?.Invoke(mapIndex, waypoints[currentWaypointIndex]);
            currentWaypointIndex++;
        }
    }
}