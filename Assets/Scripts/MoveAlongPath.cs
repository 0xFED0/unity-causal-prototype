// Copyright 2024, Fedir Khodchenko, All rights reserved.
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class MoveAlongPath : MonoBehaviour
{
    public UnityEvent OnFinished;
    public UnityEvent OnStarted;

    [SerializeField] Path m_Path;
    [SerializeField] float Speed = 1f;
    [SerializeField] public float StartOffset = 1.0f;
    [SerializeField] public float EndOffset = 0f;
    [SerializeField] float JumpHeight = 0f;
    [SerializeField] float JumpPeriod = 1f;

    [HideInInspector] public float YPos = 0f;

    float phase = 0.0f;

    CancellationTokenSource cancelMovingTracking = new CancellationTokenSource();

    public void ToStart()
    {
        phase = StartOffset;
        Place();
        OnStarted.Invoke();
    }

    public void OnDisable()
    {
        cancelMovingTracking.Cancel();
    }

    public void OnEnable()
    {
        cancelMovingTracking = new CancellationTokenSource();
    }

    public async Task<bool> MovingUntilFinish(bool fromStart = false)
    {
        if (fromStart)
            ToStart();

        if (phase >= m_Path.Length - EndOffset)
            return true;

        enabled = true;
        try
        {
            await OnFinished.Wait(cancelMovingTracking.Token);
        }
        catch (TaskCanceledException)
        {
            enabled = false;
            return false;
        }
        
        enabled = false;

        return true;
    }

    void FixedUpdate()
    {
        if (phase >= m_Path.Length - EndOffset)
        {
            OnFinished.Invoke();
            return;
        }

        phase += Speed * Time.deltaTime;
        phase = Mathf.Clamp(phase, StartOffset, m_Path.Length - EndOffset);
        Place();            
    }

    void Place()
    {
        var new_pos = m_Path.Origin + m_Path.Direction.XZ(0f) * phase;
        float jump_phase = (phase / Speed % JumpPeriod) / JumpPeriod;
        float y = YPos + Mathf.Sin(jump_phase * Mathf.PI) * JumpHeight;
        transform.position = new_pos.XZ(y);
    }
}
