using Newtonsoft.Json;
using System.Collections.Generic;

namespace NewSaveSystem
{

    [System.Serializable]
    public class SaveData
    {
        [JsonProperty]
        public Dictionary<string, object> data = new();
    }
}
