using System.Collections;
using UnityEngine;

namespace SaveSystem
{
    public interface ISerializer
	{
		string Serialize<T>(T data);
		T Deserialize<T>(string json);
	}
}
