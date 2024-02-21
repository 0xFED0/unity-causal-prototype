// Copyright 2024, Fedir Khodchenko, All rights reserved.
using UnityEngine;
using UnityEngine.Events;

public class VolumeTransfer : MonoBehaviour
{
    [SerializeField] Transform From;
    [SerializeField] Transform To;

    [SerializeField] float Speed = 0.01f;
    [SerializeField] float Minimal = 0.1f;

    public UnityEvent OnInsufficient;

    float initialFrom = 1f;

    private void Awake()
    {
        initialFrom = From.transform.localScale.x;
    }

    void OnEnable()
    {
        if (!CheckInsufficient())
            return;
        To.transform.localScale = Vector3.zero;
        Transfer(Minimal);
    }

    void Update()
    {
        if (!CheckInsufficient())
            return;
        Transfer(Speed * Time.deltaTime);
    }

    void Transfer(float amount)
    {
        float from = From.transform.localScale.x - amount;
        float to = To.transform.localScale.x + amount;
        From.transform.localScale = new Vector3(from, from, from);
        To.transform.localScale = new Vector3(to, to, to);
        CheckInsufficient();
    }

    bool CheckInsufficient()
    {
        bool good = From.localScale.x > Minimal;
        if (!good)
            OnInsufficient.Invoke();
        return good;
    }

    public void Revert()
    {
        float amount = To.transform.localScale.x;
        float from = From.transform.localScale.x + amount;
        From.transform.localScale = new Vector3(from, from, from);
    }

    public void OnRestart()
    {
        From.transform.localScale = new Vector3(initialFrom, initialFrom, initialFrom);
    }
}
