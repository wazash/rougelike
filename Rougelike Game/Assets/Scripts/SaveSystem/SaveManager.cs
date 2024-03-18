using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace NewSaveSystem
{
    public static class SaveManager
    {
        public static SaveData CurrentSaveData = new(); // This is the data container for saving and loading game data.
        public static List<ISaveable> Saveables = new();// This is a list of all saveable objects in the scene.

        public const string DIRECTIRY = "/SaveData/";   // This is the directory where the save file will be stored.
        public const string FILENAME = "SaveGame.json"; // This is the name and file type of the save file.

        /// <summary>
        /// Register a saveable object to the SaveManager. This is called in the Awake() method of the saveable object.
        /// </summary>
        /// <param name="saveable"></param>
        public static void RegisterSaveable(ISaveable saveable)
        {
            if (!Saveables.Contains(saveable))
                Saveables.Add(saveable);
        }

        /// <summary>
        /// Save the game data to a file. This is called when the player wants to save the game.
        /// </summary>
        /// <returns></returns>
        public static bool SaveGame()
        {
            foreach (ISaveable saveable in Saveables)
            {
                string id = saveable.GetSaveID();
                CurrentSaveData.data[id] = saveable.Save();
            }

            string dir = Application.persistentDataPath + DIRECTIRY;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            JsonConverter[] converters = { new Vector3JsonConverter(), new Vector2JsonConverter() };
            string json = JsonConvert.SerializeObject(CurrentSaveData, Formatting.Indented, converters);
            File.WriteAllText(dir + FILENAME, json);

            return true;
        }

        /// <summary>
        /// Load the game data from a file. This is called when the player wants to load the game.
        /// </summary>
        public static void LoadGame()
        {
            string fullPath = Application.persistentDataPath + DIRECTIRY + FILENAME;

            if (File.Exists(fullPath))
            {
                string json = File.ReadAllText(fullPath);
                JsonConverter[] converters = { new Vector3JsonConverter(), new Vector2JsonConverter() };
                CurrentSaveData = JsonConvert.DeserializeObject<SaveData>(json, converters);

                var saveablesCopy = new List<ISaveable>(Saveables);

                foreach (ISaveable saveable in saveablesCopy)
                {
                    string id = saveable.GetSaveID();
                    if (CurrentSaveData.data.TryGetValue(id, out object saveDataJson))
                    {
                        var saveData = JsonConvert.DeserializeObject(saveDataJson.ToString(), saveable.GetDataType());
                        saveable.Load(saveData);
                    }
                }
            }
            else
            {
                Debug.LogError("Save file not found");
            }
        }
    }
}
