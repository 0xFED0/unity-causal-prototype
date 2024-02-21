// Copyright 2024, Fedir Khodchenko, All rights reserved.
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Collider))]
public class Infectible : MonoBehaviour
{
    public UnityEvent OnBlowUpStart;
    [SerializeField] float TimeToBlowUp = 1f;
    [SerializeField] Material infected;
    Material initialMaterial;

    private void Awake()
    {
        initialMaterial = GetComponent<Renderer>().material;
    }

    public void InfectAndPropagate(float radius)
    {
        var overlapped = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask("Obstacles"));
        foreach(var col in overlapped)
        {
            if (col.TryGetComponent<Infectible>(out var inf_obj))
                inf_obj.Infect();
        }
        
    }

    void Infect()
    {
        GetComponent<Renderer>().material = infected;
        Invoke(nameof(BlowUp), TimeToBlowUp);
    }

    void BlowUp()
    {
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        OnBlowUpStart.Invoke();
    }

    public void OnRestart()
    {
        GetComponent<Collider>().enabled = true;

        var render = GetComponent<Renderer>();
        render.enabled = true;
        render.material = initialMaterial;
    }
}
