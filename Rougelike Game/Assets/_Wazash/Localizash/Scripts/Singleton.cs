using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static bool IsApplicationQuiting = false;

    public static T Instance
    {
        get
        {
            if (IsApplicationQuiting)
            {
                Debug.LogWarning($"[Singleton] Instance {typeof(T)} already destroyed. Returning null.");
                return null;
            }

            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if(FindObjectsOfType(typeof (T)).Length > 1)
                {
                    Debug.LogError($"[Singleton] Something went really wrong - there should never be more than 1 singleton! Reopening the scene might fix it.");
                    return instance;
                }

                if(instance == null)
                {
                    GameObject singletonObject = new();
                    instance = singletonObject.AddComponent<T>();
                    singletonObject.name = typeof(T).ToString() + $" (Singleton)";

                    DontDestroyOnLoad(singletonObject);
                }
            }

            return instance;
        }
    }

    protected virtual void OnDestroy()
    {
        //IsApplicationQuiting = true;
    }
}
