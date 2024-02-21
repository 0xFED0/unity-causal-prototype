// Copyright 2024, Fedir Khodchenko, All rights reserved.
using UnityEngine;

public class DoorAnim : MonoBehaviour
{
    public void OnVictory()
    {
        GetComponent<Animation>().Play();
    }

    public void OnRestart()
    {
        var animation = GetComponent<Animation>();
        animation.Rewind();
        animation.Play();
        animation.Sample();
        animation.Stop();
    }
}
