using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersMover : MonoBehaviour
{
    [SerializeField] private PlayerWayBuilder _wayBuilder;
    [SerializeField] private PlayersSpawner _playerSpawner;
    [SerializeField] private MapSpawner _mapSpawner;
    [SerializeField] private float _duration;

    private List<Coroutine> _moveCoroutines = new();
    private bool _isMoving;

    public void TryStartMove(GameMapVector2 direction)
    {
        if (_isMoving)
            return;
        
        foreach (var coroutine in _moveCoroutines)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
        }
        
        _moveCoroutines.Clear();

        _isMoving = true;

        for (int i = 0; i < _playerSpawner.PlayersCount; i++)
        {
            List<Transform> waypoints = _wayBuilder.SearchWay(i, direction, _mapSpawner.SearchPlayer(i));
            
            var coroutine = StartCoroutine(MovePlayer(i, _playerSpawner.GetPlayerTransform(i), waypoints, _duration));
            
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
            _mapSpawner.ChangeItem(mapIndex, waypoints[currentWaypointIndex]);
            currentWaypointIndex++;
        }
        
        //_mapSpawner.
    }
}
