using UnityEngine;

namespace NewSaveSystem
{
    [System.Serializable]
    public struct Vector3Json
    {
        public float x;
        public float y;
        public float z;

        public Vector3Json(Vector3 v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }

        public static implicit operator Vector3(Vector3Json v) => new(v.x, v.y, v.z); // Convert Vector3Json to Vector3
        public static implicit operator Vector3Json(Vector3 v) => new(v); // Convert Vector3 to Vector3Json
    }
}
