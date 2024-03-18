using UnityEngine;

namespace NewSaveSystem
{
    [System.Serializable]
    public struct Vector2Json
    {
        public float x;
        public float y;

        public Vector2Json(Vector2 v)
        {
            x = v.x;
            y = v.y;
        }

        public static implicit operator Vector2(Vector2Json v) => new(v.x, v.y);   // Convert Vector2Json to Vector2
        public static implicit operator Vector2Json(Vector2 v) => new(v);          // Convert Vector2 to Vector2Json
    }
}
