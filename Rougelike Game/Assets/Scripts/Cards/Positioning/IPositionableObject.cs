using UnityEngine;

namespace Cards
{
    public interface IPositionableObject
    {
        RectTransform RectTransform { get; }
        Vector2 OriginalSizeDelta { get; }
    }
}