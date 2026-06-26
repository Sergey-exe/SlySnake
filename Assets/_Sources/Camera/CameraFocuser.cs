using System;
using UnityEngine;
using System.Collections.Generic;

public class CameraFocuser : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _padding = 1f;
    [SerializeField] private float _minOrthographicSize = 5f;
    [SerializeField] private float _maxOrthographicSize = 50f;
    [SerializeField] private float _cameraZOffset = -10f;

    public void FocusCameraOnItems(List<Transform> items)
    {
        if (items == null || items.Count == 0) 
            throw new ArgumentNullException();
        
        Vector3 sumPositions = Vector3.zero;
        
        foreach (var item in items)
            sumPositions += item.position;
        
        
        Vector3 center = sumPositions / items.Count;
        center.z = _cameraZOffset;

        float maxDistance = 0f;
        
        foreach (var item in items)
        {
            float dist = Vector2.Distance(new Vector2(item.position.x, item.position.y),
                new Vector2(center.x, center.y));
            
            if (dist > maxDistance)
                maxDistance = dist;
        }
        
        float radius = maxDistance + _padding;
        
        float aspectRatio = (float)Screen.width / Screen.height;
        
        float sizeBasedOnRadiusVertical = radius;
        float sizeBasedOnRadiusHorizontal = radius / aspectRatio;
        float requiredSize = Mathf.Max(sizeBasedOnRadiusVertical, sizeBasedOnRadiusHorizontal);
        
        requiredSize = Mathf.Clamp(requiredSize, _minOrthographicSize, _maxOrthographicSize);
        
        _camera.transform.position = center;
        _camera.orthographicSize = requiredSize;
    }
}