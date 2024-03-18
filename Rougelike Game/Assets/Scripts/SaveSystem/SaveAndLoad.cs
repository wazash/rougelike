using NewSaveSystem;
using Sirenix.OdinInspector;
using UnityEngine;

public class SaveAndLoad : MonoBehaviour
{
    [Button]
    public void Save()
    {
        SaveManager.SaveGame();
    }

    [Button]
    public void Load()
    {
        SaveManager.LoadGame();
    }
}
