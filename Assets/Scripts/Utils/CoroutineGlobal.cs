using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineGlobal : MonoBehaviour
{
    private static CoroutineGlobal instance;
    public static CoroutineGlobal Handle => instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Initialize()
    {
        if (instance == null)
        {
            instance = new GameObject("Coroutine Handler").AddComponent<CoroutineGlobal>();
            DontDestroyOnLoad(instance.gameObject);
        }
    }
}
