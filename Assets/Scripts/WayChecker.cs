// Copyright 2024, Fedir Khodchenko, All rights reserved.
using UnityEngine;

public class WayChecker : MonoBehaviour
{
    [SerializeField] Path m_Path;
    [SerializeField] float ChangeMinDiff = 0.01f;

    [HideInInspector] public float EndOffset = 0f;
    [HideInInspector] public float FreeDist { get; private set; } = 0f;
    [HideInInspector] public float FreeBackDist => Length - FreeDist;
    [HideInInspector] public float Length => m_Path.Length;
    [HideInInspector] public bool WayCompleted => AllClear && Length < EndOffset + ChangeMinDiff;
    [HideInInspector] public bool AllClear { get; private set; } = false;

    public bool CheckChanges()
    {
        if (Length < EndOffset + ChangeMinDiff)
        {
            AllClear = true;
            return true;
        }

        bool changed = false;

        float radius = m_Path.Width / 2;
        AllClear = !Physics.SphereCast(m_Path.FromPoint, radius, m_Path.Direction, out var hit, Length - EndOffset, LayerMask.GetMask("Obstacles"));
        if (!AllClear)
        {
            float dist = Mathf.Clamp(hit.distance, 0, Length - EndOffset);
            changed = !FreeDist.SameAs(hit.distance, ChangeMinDiff);
            FreeDist = hit.distance;
        }
        else
        {
            FreeDist = Length;
            return true;
        }

        return changed;
    }
}
