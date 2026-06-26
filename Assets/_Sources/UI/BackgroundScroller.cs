using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private Vector2 _speed = new Vector2(0.05f, 0.05f);
    private RawImage _image;

    private void Awake()
    {
        _image = GetComponent<RawImage>();
    }

    private void Update()
    {
        Rect uvRect = _image.uvRect;
        uvRect.position += _speed * Time.deltaTime;
        _image.uvRect = uvRect;
    }
}