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

        public const string DIRECTORY = "/SaveData/";   // This is the directory where the save file will be stored.
        public const string FILENAME = "SaveGame.json"; // This is the name and file type of the save file.

        public static string DirPath => Application.persistentDataPath + DIRECTORY;
        public static string SavePath => Application.persistentDataPath + DIRECTORY + FILENAME;

        /// <summary>
        /// Register a saveable object to the SaveManager. This is called in the Awake() method of the saveable object.
        /// </summary>
        /// <param name="saveable"></param>
        public static void RegisterSaveable(ISaveable saveable)
        {
            if (!Saveables.Contains(saveable))
                Saveables.Add(saveable);
        }

        public static void UnregisterSaveable(ISaveable saveable)
        {
            if (Saveables.Contains(saveable))
                Saveables.Remove(saveable);
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
                Debug.Log(id);
            }

            string dir = DirPath;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            JsonConverter[] converters =
                {
                new Vector3JsonConverter(),
                new Vector2JsonConverter()
            };

            JsonSerializerSettings settings = new()
            {
                Formatting = Formatting.Indented,
                Converters = converters,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            string json = JsonConvert.SerializeObject(CurrentSaveData, settings);
            File.WriteAllText(SavePath, json);
            Debug.Log($"Game Saved in:{dir + FILENAME}");

            return true;
        }

        /// <summary>
        /// Load the game data from a file. This is called when the player wants to load the game.
        /// </summary>
        public static void LoadGame()
        {
            if (File.Exists(SavePath))
            {
                string json = File.ReadAllText(SavePath);

                JsonConverter[] converters =
                {
                    new Vector3JsonConverter(),
                    new Vector2JsonConverter()
                };
                CurrentSaveData = JsonConvert.DeserializeObject<SaveData>(json, converters);

                var saveablesCopy = new List<ISaveable>(Saveables);

                foreach (ISaveable saveable in saveablesCopy)
                {
                    string id = saveable.GetSaveID();
                    if (CurrentSaveData.data.TryGetValue(id, out object saveDataJson))
                    {
                        var saveData = JsonConvert.DeserializeObject(saveDataJson.ToString(), saveable.GetDataType());
                        saveable.Load(saveData);
                        Debug.Log($"Loaded {id}");
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
