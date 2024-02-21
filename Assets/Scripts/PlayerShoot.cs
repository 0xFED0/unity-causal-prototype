// Copyright 2024, Fedir Khodchenko, All rights reserved.
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

[RequireComponent(typeof(Renderer))]
public class PlayerShoot : MonoBehaviour
{
    public UnityEvent AfterShot;

    [SerializeField] BulletHit Bullet;
    [SerializeField] VolumeTransfer VolTransfer;
    [SerializeField] Material FailMaterial;
    MoveAlongPath BulletMove;
    Renderer BulletRender;
    Material initialMaterial;

    bool bulletBusy = false;
    bool bulletFired = false;
    bool failed = false;

    void Awake()
    {
        initialMaterial = GetComponent<Renderer>().material;
        SetBullet(Bullet);
        DropBullet();
    }

    public void SetBullet(BulletHit bullet)
    {
        var move = bullet.GetComponent<MoveAlongPath>();
        var render = bullet.GetComponent<Renderer>();

        Assert.IsNotNull(move);
        Assert.IsNotNull(render);

        BulletMove = move;
        BulletRender = render;
    }

    public void BeginBullet()
    {
        if (!enabled || bulletBusy || failed)
            return;

        BulletMove.enabled = false;
        BulletMove.YPos = transform.position.y;
        BulletMove.ToStart();

        BulletRender.enabled = true;
        VolTransfer.enabled = true;

        bulletBusy = true;
        bulletFired = false;
    }

    public void DropBullet()
    {
        if (failed || (!bulletFired && bulletBusy))
            VolTransfer.Revert();

        Bullet.enabled = false;
        BulletRender.enabled = false;
        BulletMove.enabled = false;
        VolTransfer.enabled = false;

        BulletMove.ToStart();

        bulletBusy = false;
    }

    public void FireBullet()
    {
        if (bulletFired || !bulletBusy || failed)
            return;

        Bullet.OnHit.AddOneShot(BulletOnHit);
        Bullet.OnLifeEnd.AddOneShot(BulletLifeEnd);
        Bullet.enabled = true;
        Bullet.transform.localPosition = Bullet.transform.localPosition.XZ(Bullet.Radius);
        BulletMove.YPos = Bullet.transform.position.y;
        BulletMove.enabled = true;
        VolTransfer.enabled = false;

        bulletFired = true;
    }

    public void OnFail()
    {
        failed = true;
        DropBullet();
        GetComponent<Renderer>().material = FailMaterial;
    }

    public void OnRestart()
    {
        bulletBusy = false;
        bulletFired = false;
        failed = false;

        DropBullet();

        GetComponent<Renderer>().material = initialMaterial;
    }

    void BulletOnHit()
    {
        BulletMove.enabled = false;
        Bullet.enabled = false;
    }

    void BulletLifeEnd()
    {
        AfterShot.Invoke();
        DropBullet();
    }
}
