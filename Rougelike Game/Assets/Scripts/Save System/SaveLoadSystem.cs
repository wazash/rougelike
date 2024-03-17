using Map;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;

namespace SaveSystem
{
    [System.Serializable]
    public class GameData
    {
        public string Name;
        public string CurrentLevelName;
        public List<NodeData> Nodes;
    }

    public interface ISaveable
    {
        string Id { get; set; }
    }

    public interface IBind<TData> where TData : ISaveable
    {
        string Id { get; }
        void Bind(TData data);
    }

    public class SaveLoadSystem : Singleton<SaveLoadSystem> 
    {
        public GameData gameData;

        IDataService dataService;

        private void Awake()
        {
            dataService = new FileDataService(new JsonSerializer());
        }

        private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
        private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Bind<Node, NodeData>(gameData.Nodes);
        }

        public void Bind<T, TData>(TData data) where T : MonoBehaviour, IBind<TData> where TData : ISaveable, new()
        {
            var entity = FindObjectsByType<T>(FindObjectsSortMode.None).FirstOrDefault();
            if (entity != null)
            {
                if (data != null)
                {
                    data = new TData { Id = entity.Id };
                }
                entity.Bind(data);
            }
        }

        public void Bind<T, TData>(List<TData> datas) where T : MonoBehaviour, IBind<TData> where TData : ISaveable, new()
        {
            var entities = FindObjectsByType<T>(FindObjectsSortMode.None);

            foreach (var entity in entities)
            {
                var data = datas.FirstOrDefault(data => data.Id == entity.Id);
                if (data == null)
                {
                    data = new TData { Id = entity.Id };
                    datas.Add(data);
                }
                entity.Bind(data);
            }
        }

        public void NewGame()
        {
            gameData = new() 
            { 
                Name = "New Game",
                CurrentLevelName = "Game"
            };

            SceneManager.LoadScene(gameData.CurrentLevelName);
        }

        public void SaveGame()
        {
            dataService.Save(gameData);
        }

        public void LoadGame(string gameName)
        {
            gameData = dataService.Load(gameName);

            if(String.IsNullOrWhiteSpace(gameData.CurrentLevelName))
            {
                gameData.CurrentLevelName = "Game";
            }

            SceneManager.LoadScene(gameData.CurrentLevelName);
        }

        public void ReloadGame() => LoadGame(gameData.Name);

        public void DeleteGame(string gameName) => dataService.Delete(gameName);
    }
}
