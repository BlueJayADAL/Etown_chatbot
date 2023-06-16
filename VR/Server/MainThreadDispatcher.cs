using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{
    private static MainThreadDispatcher instance;
    private readonly Queue<System.Action> actions = new Queue<System.Action>();

    public static MainThreadDispatcher Instance()
    {
        if (instance == null)
        {
            var go = new GameObject("MainThreadDispatcher");
            instance = go.AddComponent<MainThreadDispatcher>();
            DontDestroyOnLoad(go);
        }

        return instance;
    }

    private void Update()
    {
        lock (actions)
        {
            while (actions.Count > 0)
            {
                var action = actions.Dequeue();
                action.Invoke();
            }
        }
    }

    public void Enqueue(System.Action action)
    {
        lock (actions)
        {
            actions.Enqueue(action);
        }
    }
}