// Copyright 2024, Fedir Khodchenko, All rights reserved.
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(MoveAlongPath))]
public class BulletHit : MonoBehaviour
{
    public UnityEvent OnHit;
    public UnityEvent OnLifeEnd;

    [HideInInspector] public float Radius => radius * transform.lossyScale.x;

    float radius = 0f;

    private void Awake()
    {
        var colider = GetComponent<SphereCollider>();
        radius = colider.radius;
        GetComponent<MoveAlongPath>().OnFinished.AddListener(LifeEnd);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled)
            return;
        
        OnHit.Invoke();

        if (other.TryGetComponent<Infectible>(out var inf_obj))
            Infect(inf_obj);
        else
            LifeEnd();
    }

    void LifeEnd()
    {
        OnLifeEnd.Invoke();
    }

    void Infect(Infectible infected)
    {
        infected.OnBlowUpStart.AddOneShot(LifeEnd);
        infected.InfectAndPropagate(Radius);
    }
}
