using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView
{

    PlayerModel _myModel;

    MeshTrail _meshTrail;
    PlayerAnimations _anims;
    PlayerParticles _particles;

    PlayerTrail _trail;
    float _trailTime;

    BasePlayer _player;

    float _deathDuration;
    Material _material;

    public PlayerView(PlayerModel model, 
        ObjectPool<GameObject> pool, 
        SkinnedMeshRenderer[] skMeshRenderer, 
        Material trailShader, 
        string matAlphaName, 
        float trailTime,
        Animator animator, 
        string idle, 
        string walk, 
        string jump, 
        string fall, 
        BasePlayer player, 
        Transform mesh, 
        PlayerTrail trail, 
        float deathDur, 
        ParticleSystem diveParticles,
        ParticleSystem diveLandParticles)
    {
        _myModel = model;

        _trailTime = trailTime;
        _trail = trail;
        _player = player;

        _material = skMeshRenderer[0].material;
        _meshTrail = new MeshTrail(this, pool, skMeshRenderer, trailShader, matAlphaName, trailTime);
        _anims = new PlayerAnimations(player ,animator, mesh, walk, idle, fall, jump);
        _particles = new PlayerParticles(diveParticles, diveLandParticles, _myModel);

        _deathDuration = deathDur;
    }

    public void FakeStart()
    {
        _meshTrail.FakeStart();
        _anims.FakeStart();
        _particles.FakeStart();

        EventManager.Subscribe("OnDashEnter", TrailOn);
        EventManager.Subscribe("OnPowerDashEnter", TrailOn);
        EventManager.Subscribe("OnDiveEnter", TrailOn);

        //_player.OnDieEnter += Die;
        EventManager.Subscribe("OnDieEnter", Die);

        //_player.OnRespawn += RestoreAlpha;
        EventManager.Subscribe("OnRespawn", RestoreAlpha);
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
        _particles.FakeOnDestroy();


        EventManager.Unsubscribe("OnDashEnter", TrailOn);
        EventManager.Unsubscribe("OnPowerDashEnter", TrailOn);
        EventManager.Unsubscribe("OnDiveEnter", TrailOn);
        EventManager.Unsubscribe("OnDieEnter", Die);
        EventManager.Unsubscribe("OnRespawnEnter", TrailOn);
    }

    void TrailOn(params object[] nada)
    {
        if (_trail == null) return;

        _trail.Active = true;

        _myModel.StartCoroutine(TrailOff(_trailTime));
    } 

    IEnumerator TrailOff(float wait)
    {
        yield return new WaitForSeconds(wait);

        _trail.Active = false;
    }

    public void Die(params object[] noSeUsa)
    {
        _myModel.StartCoroutine(DisolvePlayer(_deathDuration));
    }

    IEnumerator DisolvePlayer(float death)
    {
        var time = death;
        var alpha = 1.0f;
        while(time >= 0) 
        {
            time -= 0.1f;

             alpha -= 0.1f * death;

            //Debug.Log(alpha);

            _material.SetFloat("_Alpha", alpha);

            yield return new WaitForSeconds(0.1f);
        }
  
        RestoreAlpha();
    }

    void RestoreAlpha(params object[] NoSeUsa)
    {
        _material.SetFloat("_Alpha", 1);
    }

}
