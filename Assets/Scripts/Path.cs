// Copyright 2024, Fedir Khodchenko, All rights reserved.
using UnityEngine;

[ExecuteInEditMode]
public class Path : MonoBehaviour
{
    [SerializeField] Transform From;
    [SerializeField] Transform To;

    public float Width = 1f;

    [HideInInspector] public float Length { get; private set; } = 0f;
    [HideInInspector] public Vector3 Direction => transform.forward;
    [HideInInspector] public Vector3 Origin => transform.position;

    [HideInInspector] public Vector3 FromPoint => From.position;
    [HideInInspector] public Vector3 ToPoint => To.position;

    private void Awake()
    {
        UpdatePath();
    }

    private void Update()
    {
        if (Application.isEditor && !Application.isPlaying)
            UpdatePath();
    }

    public void UpdatePath()
    {
        Vector3 fromPos = From.position.XZ(transform.position.y);
        Vector3 toPos = To.position.XZ(transform.position.y);
        Length = From.position.XZDistanceTo(To.position);
        if (Length > 0)
        {
            transform.localScale = new Vector3(Width, 1f, Length);
            transform.position = fromPos;
            transform.LookAt(toPos, Vector3.up);
        }
    }
}
