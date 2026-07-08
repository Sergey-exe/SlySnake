using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class LBPlayerDataCustomizer : MonoBehaviour
{
    [SerializeField] private Image _crownImage;

    private readonly Color _goldColor = HexToColor("#FFF100");
    private readonly Color _silverColor = HexToColor("#D1D1D1");
    private readonly Color _bronzeColor = HexToColor("#CD7F32");

    private void OnRectTransformDimensionsChange()
    {
        if (TryGetComponent<LBPlayerDataYG>(out var playerData) && playerData.data != null)
        {
            string rank = playerData.data.rank;

            if (rank == "1")
                ApplyColor(_goldColor);
            else if (rank == "2")
                ApplyColor(_silverColor);
            else if (rank == "3")
                ApplyColor(_bronzeColor);
        }
    }

    private void ApplyColor(Color targetColor)
    {
        if (_crownImage != null) 
            _crownImage.color = targetColor;
    }

    private static Color HexToColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
            return color;

        return Color.white;
    }
}