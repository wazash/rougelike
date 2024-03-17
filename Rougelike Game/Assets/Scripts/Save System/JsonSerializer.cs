using Newtonsoft.Json;
using UnityEngine;

namespace SaveSystem
{
    public class JsonSerializer : ISerializer
    {
        public string Serialize<T>(T data)
        {
            //return JsonConvert.SerializeObject(data);
            return JsonUtility.ToJson(data);
        }

        public T Deserialize<T>(string json)
        {
            //return JsonConvert.DeserializeObject<T>(json);
            return JsonUtility.FromJson<T>(json);
        }
    }
}
