using System;
using UnityEngine;
using System.Collections.Generic;

public class CameraFocuser : MonoBehaviour
{
    [SerializeField] private Camera Camera;
    [SerializeField] private float Padding = 1f;
    [SerializeField] private float MinOrthographicSize = 5f;
    [SerializeField] private float MaxOrthographicSize = 50f;
    [SerializeField] private float CameraZOffset = -10f;

    public void FocusCameraOnItems(List<Transform> items)
    {
        if (items == null || items.Count == 0) 
            throw new ArgumentNullException();
        
        Vector3 sumPositions = Vector3.zero;
        
        foreach (var item in items)
            sumPositions += item.position;
        
        
        Vector3 center = sumPositions / items.Count;
        center.z = CameraZOffset;

        float maxDistance = 0f;
        
        foreach (var item in items)
        {
            float dist = Vector2.Distance(new Vector2(item.position.x, item.position.y),
                new Vector2(center.x, center.y));
            
            if (dist > maxDistance)
                maxDistance = dist;
        }
        
        float radius = maxDistance + Padding;
        
        float aspectRatio = (float)Screen.width / Screen.height;
        
        float sizeBasedOnRadiusVertical = radius;
        float sizeBasedOnRadiusHorizontal = radius / aspectRatio;
        float requiredSize = Mathf.Max(sizeBasedOnRadiusVertical, sizeBasedOnRadiusHorizontal);
        
        requiredSize = Mathf.Clamp(requiredSize, MinOrthographicSize, MaxOrthographicSize);
        
        Camera.transform.position = center;
        Camera.orthographicSize = requiredSize;
    }
}