using Sirenix.OdinInspector;
using System;
using UnityEditor;
using UnityEngine;

namespace NewSaveSystem
{
    public class PoI : MonoBehaviour, ISaveable
    {
        [ShowInInspector] private PoIData MyData = new();
        [SerializeField] private bool visited;

        private void Awake()
        {
            if (string.IsNullOrEmpty(MyData.SaveId))
                MyData.SaveId = GUID.Generate().ToString();

            SaveGameManager.RegisterSaveable(this);
        }

        public Type GetDataType() => typeof(PoIData);

        public string GetSaveID() => MyData.SaveId;

        public object Save()
        {
            MyData.Position = transform.position;
            MyData.Visited = visited;

            return MyData;
        }

        public void Load(object saveData)
        {
            MyData = (PoIData)saveData;

            transform.position = MyData.Position;
            visited = MyData.Visited;
        }
    }

    [Serializable]
    public struct PoIData
    {
        public string SaveId;
        public Vector3 Position;
        public bool Visited;
    }
}
