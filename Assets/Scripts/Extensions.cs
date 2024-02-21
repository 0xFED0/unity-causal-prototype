// Copyright 2024, Fedir Khodchenko, All rights reserved.
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public static class Extensions
{
    public static Vector3 XZ(this Vector3 vv, float y)
        => new Vector3(vv.x, y, vv.z);

    public static Vector2 XZ(this Vector3 vv)
        => new Vector2(vv.x, vv.z);

    public static float XZDistanceTo(this Vector3 a, Vector3 b)
        => Vector2.Distance(a.XZ(), b.XZ());

    public static bool SameAs(this float a, float other, float eps = float.Epsilon)
        => Mathf.Abs(a - other) < eps;

    public static void AddOneShot(this UnityEvent ev, UnityAction call)
    {
        void complete()
        {
            ev.RemoveListener(complete);
            call();
        }
        ev.AddListener(complete);
    }

    public static Task Wait(this UnityEvent ev)
        => ev.CompletionSource().Task;

    public static Task Wait(this UnityEvent ev, CancellationToken token)
    {
        var source = ev.CompletionSource();
        token.Register(() => {
            if (!source.Task.IsCompleted)
                source.SetCanceled();
        });
        return source.Task;
    }

    private static TaskCompletionSource<object> CompletionSource(this UnityEvent ev)
    {
        var source = new TaskCompletionSource<object>();
        ev.AddOneShot(() => {
            if (!source.Task.IsCompleted)
                source.SetResult(null);
        });
        return source;
    }
}
