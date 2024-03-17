using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace NewSaveSystem
{

    public static class SaveGameManager
    {
        public static SaveData CurrentSaveData = new();
        public static List<ISaveable> Saveables = new();

        public const string DIRECTIRY = "/SaveData/";
        public const string FILENAME = "SaveGame.json";

        public static void RegisterSaveable(ISaveable saveable)
        {
            if(!Saveables.Contains(saveable))
                Saveables.Add(saveable);
        }

        public static bool SaveGame()
        {
            foreach(ISaveable saveable in Saveables)
            {
                saveable.PopulateSaveData(CurrentSaveData);
            }

            string dir = Application.persistentDataPath + DIRECTIRY;
            if(!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string json = JsonConvert.SerializeObject(CurrentSaveData, Formatting.Indented, new Vector3JsonConverter());
            //string cipher = EncryptionHelper.EncryptString(json);
            File.WriteAllText(dir + FILENAME, json);

            return true;
        }

        public static void LoadGame()
        {
            string fullPath = Application.persistentDataPath + DIRECTIRY + FILENAME;

            if (File.Exists(fullPath))
            {
                string json = File.ReadAllText(fullPath);
                //string json = EncryptionHelper.DecryptString(cipher);
                CurrentSaveData = JsonConvert.DeserializeObject<SaveData>(json, new Vector3JsonConverter());

                foreach (ISaveable saveable in Saveables)
                {
                    saveable.LoadFromSaveData(CurrentSaveData);
                }
            }
            else
            {
                Debug.LogError("Save file not found");
            }
        }
    }
}
