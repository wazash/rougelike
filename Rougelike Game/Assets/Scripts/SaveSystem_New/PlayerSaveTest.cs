using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace NewSaveSystem
{
    public class PlayerSaveTest : MonoBehaviour, ISaveable
    {

        [SerializeField] private string id;

        [ShowInInspector] private PlayerData MyData = new();

        private void Awake()
        {
            if (string.IsNullOrEmpty(id))
            {
                GenerateId();
            }

            SaveGameManager.RegisterSaveable(this);
        }

        [Button]
        private void GenerateId() => id = GUID.Generate().ToString();

        public string GetSaveID() => id;

        public object Save()
        {
            PlayerData data = new()
            {
                Id = id,
                Position = transform.position,
                Rotation = transform.eulerAngles
            };

            MyData = data;
            return data;
        }

        public void Load(object saveData)
        {
            PlayerData data = (PlayerData)saveData;

            MyData = data;
            transform.position = data.Position;
            transform.eulerAngles = data.Rotation;
        }

        public System.Type GetDataType()
        {
            return typeof(PlayerData);
        }
    }

    [System.Serializable]
    public struct PlayerData
    {
        public string Id;
        public Vector3 Position;
        public Vector3 Rotation;
    }
}
