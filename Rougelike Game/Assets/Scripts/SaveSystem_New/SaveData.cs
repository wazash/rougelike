using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEditor;

namespace NewSaveSystem
{

    [System.Serializable]
    public class SaveData
    {
        public PlayerData PlayerData = new();
    }
}
