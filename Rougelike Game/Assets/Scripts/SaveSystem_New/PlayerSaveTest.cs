using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace NewSaveSystem
{
    public class PlayerSaveTest : MonoBehaviour, ISaveable
    {
        private string id;
        [ShowInInspector] private PlayerData MyData = new();

        public void LoadFromSaveData(SaveData saveData)
        {
            MyData = saveData.PlayerData;

            transform.position = MyData.Position;
            transform.eulerAngles = MyData.Rotation;
        }

        public void PopulateSaveData(SaveData saveData)
        {
            saveData.PlayerData = MyData;
        }

        private void Start()
        {
            MyData.Id = id = GUID.Generate().ToString();
            SaveGameManager.RegisterSaveable(this);
        }

        private void Update()
        {
            MyData.Position = transform.position;
            MyData.Rotation = transform.eulerAngles;
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
