using UnityEngine;

namespace SaveSystem
{
    public class JsonSerializer : ISerializer
	{
        public string Serialize<T>(T data)
		{
            return JsonUtility.ToJson(data);
        }

        public T Deserialize<T>(string json)
		{
            return JsonUtility.FromJson<T>(json);
        }
    }
}
