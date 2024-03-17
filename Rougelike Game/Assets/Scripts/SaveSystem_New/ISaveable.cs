using System;

namespace NewSaveSystem
{
    public interface ISaveable
    {
        string GetSaveID();
        object Save();
        void Load(object saveData);
        Type GetDataType();
    }
}
