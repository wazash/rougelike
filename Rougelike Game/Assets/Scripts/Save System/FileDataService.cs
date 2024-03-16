using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SaveSystem
{
    public class FileDataService : IDataService
    {
        private ISerializer serializer;
        string dataPath;
        string fileExtension;

        public FileDataService(ISerializer serializer)
        {
            this.dataPath = Application.persistentDataPath;
            this.fileExtension = "json";
            this.serializer = serializer;
        }

        private string GetFilePath(string name)
        {
            return Path.Combine(dataPath, string.Concat(name, ".", fileExtension));
        }

        public void Save(GameData data, bool overwrite = true)
        {
            string filePath = GetFilePath(data.Name);

            if(!overwrite && File.Exists(filePath))
            {
                throw new IOException($"The file: '{data.Name}.{fileExtension}' already exists and cannot be overwritten.");
            }

            File.WriteAllText(filePath, serializer.Serialize(data));
        }

        public GameData Load(string name)
        {
            string filePath = GetFilePath(name);

            if(!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file: '{name}.{fileExtension}' does not exist.");
            }

            return serializer.Deserialize<GameData>(File.ReadAllText(filePath));
        }

        public void Delete(string name)
        {
            string filePath = GetFilePath(name);

            if(File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public void DeleteAll()
        {
            foreach(string file in Directory.GetFiles(dataPath)) // Maybe with the fileExtension
            {
                File.Delete(file);
            }
        }

        public IEnumerable<string> ListSaves()
        {
            foreach(string path in Directory.EnumerateFiles(dataPath)) // Maybe with the fileExtension
            {
                if(Path.GetExtension(path) == fileExtension)
                {
                    yield return Path.GetFileNameWithoutExtension(path);
                }
            }
        }
    }
}
