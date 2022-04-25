using UnityEngine;

namespace LordAmbermaze.CellHighlight
{
    public interface ICellHighlight
    {
        void ColorTo(Color initColor, Color color, float time);
        void Init(CellHighlightManager manager);
        void SetPos(Vector2 zoneCell);
        void SetActivate(bool active);
        void PermanentColorTo(Color initColor, Color color, float time);
        void RemoveColor(float time);
        void BlinkColorTo(Color initColor, Color color, float time);
        void SetColor(Color color);
        void RemoveColor();
    }
}