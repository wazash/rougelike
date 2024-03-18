using System;

namespace NewSaveSystem
{
    /// <summary>
    /// Interface for saveable objects. Add this to any class that you want to save.
    /// </summary>
    public interface ISaveable
    {
        string GetSaveID();
        object Save();
        void Load(object saveData);
        Type GetDataType();
    }
}
