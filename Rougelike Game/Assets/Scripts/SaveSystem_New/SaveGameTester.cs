using NewSaveSystem;
using Sirenix.OdinInspector;
using UnityEngine;

public class SaveGameTester : MonoBehaviour
{
    [Button]
    public void SaveGame()
    {
        SaveGameManager.SaveGame();
    }

    [Button]
    public void LoadGame()
    {
        SaveGameManager.LoadGame();
    }
}
