using System.Collections;
using UnityEngine;

public class CoroutineRunner : Singleton<CoroutineRunner>
{
    public static Coroutine Start(IEnumerator routine) => Instance.StartCoroutine(routine);
}
