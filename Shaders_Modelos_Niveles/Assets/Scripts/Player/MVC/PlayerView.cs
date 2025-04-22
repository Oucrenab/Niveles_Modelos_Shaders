using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView
{

    PlayerModel _myModel;

    MeshTrail _meshTrail;
    PlayerAnimations _anims;

    PlayerTrail _trail;
    float _trailTime;


    public PlayerView(PlayerModel model, ObjectPool<GameObject> pool, SkinnedMeshRenderer[] skMeshRenderer, Material trailShader, string matAlphaName, float trailTime,
        Animator animator, string idle, string walk, string jump, string fall, BasePlayer player, Transform mesh, PlayerTrail trail)
    {
        _myModel = model;

        _trailTime = trailTime;
        _trail = trail;

        _meshTrail = new MeshTrail(this, pool, skMeshRenderer, trailShader, matAlphaName, trailTime);
        _anims = new PlayerAnimations(player ,animator, mesh, walk, idle, fall, jump);
    }

    public void FakeStart()
    {
        _meshTrail.FakeStart();
        _anims.FakeStart();
        EventManager.Subscribe("OnDashEnter", TrailOn);
        EventManager.Subscribe("OnPowerDashEnter", TrailOn);
        EventManager.Subscribe("OnDiveEnter", TrailOn);
    }

    public void FakeUpdate()
    {
        //Debug.Log($"<color=red>Update de View</color>");

    }

    public void StartCoroutine(IEnumerator shit)
    {
        _myModel.StartCoroutine(shit);
    }

    public void FakeOnDestroy()
    {
        _meshTrail.FakeOnDestroy();
        _anims.FakeOnDestroy();

        EventManager.Unsubscribe("OnDashEnter", TrailOn);
        EventManager.Unsubscribe("OnPowerDashEnter", TrailOn);
        EventManager.Unsubscribe("OnDiveEnter", TrailOn);
    }

    void TrailOn(params object[] nada)
    {
        if (_trail == null) return;

        _trail.Active = true;

        _myModel.StartCoroutine(TrailOf(_trailTime));
    } 

    IEnumerator TrailOf(float wait)
    {
        yield return new WaitForSeconds(wait);

        _trail.Active = false;
    }
}
