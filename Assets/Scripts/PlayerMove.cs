// Copyright 2024, Fedir Khodchenko, All rights reserved.

using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMove : MonoBehaviour
{
    public UnityEvent OnCompleted;

    [SerializeField] Path m_Path;
    [SerializeField] WayChecker Checker;
    [SerializeField] MoveAlongPath Mover;
    [SerializeField] PlayerShoot Shooting;
    [SerializeField] float SpaceBeforeObstacle = 1f;
    [SerializeField] float SpaceBeforeEnd = 1f;
    [SerializeField] float DelayBeforeMove = 0.5f;

    bool moving = false;

    public void Awake()
    {
        CorrectPosition();
    }

    public void OnRestart()
    {
        enabled = true;
        Shooting.enabled = true;
        Mover.enabled = false;
        moving = false;
        transform.localPosition = Vector3.zero;
        CorrectPosition();

        UpdPathAfterShapeReset();
    }

    public void OnFail()
    {
        enabled = false;
        Mover.enabled = false;
    }

    public void TryMoveForward()
    {
        if (!enabled || moving)
            return;

        UpdatePath();
        Checker.EndOffset = SpaceBeforeEnd;
        if (Checker.CheckChanges())
        {
            Shooting.enabled = false;
            MoveForward();
        }
    }

    async void MoveForward()
    {
        moving = true;

        await Task.Delay(TimeSpan.FromSeconds(DelayBeforeMove));

        if (Checker.AllClear)
            Mover.EndOffset = SpaceBeforeEnd;
        else
            Mover.EndOffset = Checker.FreeBackDist + SpaceBeforeObstacle;

        if (await Mover.MovingUntilFinish(fromStart: true))
        {
            UpdatePath();
            Mover.EndOffset = 0;
            Checker.CheckChanges();
        }

        if (Checker.WayCompleted)
            OnCompleted.Invoke();
        else
            Shooting.enabled = true;

        moving = false;
    }

    void UpdatePath()
    {
        CorrectPosition();
        m_Path.Width = transform.localScale.x;
        m_Path.UpdatePath();
    }

    void UpdPathAfterShapeReset()
    {
        // Run deffered after scale applied (by VolumeTransfer)
        Invoke(nameof(UpdatePath), 0f);
    }

    void CorrectPosition()
    {
        float radius = 0.5f;
        transform.localPosition = transform.localPosition.XZ(radius * transform.lossyScale.x);
        Mover.YPos = transform.position.y;
    }
}
