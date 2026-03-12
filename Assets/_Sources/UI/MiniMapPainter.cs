using UnityEngine;

public class MiniMapPainter : MonoBehaviour
{
    [SerializeField] private Camera _previewCamera;
    [SerializeField] private Transform _mapsRoot;
    [SerializeField] private float _padding = 1.1f;

    public void Paint()
    {
        Bounds bounds = CalculateBounds();

        Vector3 center = bounds.center;

        _previewCamera.transform.position =
            new Vector3(center.x, center.y, -10f);

        float sizeX = bounds.size.x / _previewCamera.aspect / 2f;
        float sizeY = bounds.size.y / 2f;

        _previewCamera.orthographicSize =
            Mathf.Max(sizeX, sizeY) * _padding;

        _previewCamera.Render();
    }

    private Bounds CalculateBounds()
    {
        Renderer[] renderers = _mapsRoot.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
            return new Bounds(Vector3.zero, Vector3.one);

        Bounds bounds = renderers[0].bounds;

        foreach (var r in renderers)
            bounds.Encapsulate(r.bounds);

        return bounds;
    }
}