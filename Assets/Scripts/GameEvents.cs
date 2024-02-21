// Copyright 2024, Fedir Khodchenko, All rights reserved.
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    [SerializeField] List<Component> BroadcastRoots = new();
    [SerializeField] UnityEvent OnVictory;
    [SerializeField] UnityEvent OnFail;
    [SerializeField] UnityEvent OnRestart;

    public void Restart(float delay = 0f)
    {
        Invoke(nameof(DoRestart), delay);
    }

    public void Fail()
    {
        OnFail.Invoke();
        Broadcast("OnFail");
    }

    public void Victory()
    {
        OnVictory.Invoke();
        Broadcast("OnVictory");
    }

    void DoRestart()
    {
        OnRestart.Invoke();
        Broadcast("OnRestart");
    }

    void Broadcast(string methodName)
    {
        foreach (var root in BroadcastRoots)
        {
            root.BroadcastMessage(methodName, SendMessageOptions.DontRequireReceiver);
        }
    }
}
