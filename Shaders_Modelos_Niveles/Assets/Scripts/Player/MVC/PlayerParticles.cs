using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerParticles
{
    [SerializeField] ParticleSystem _diveParticles;
    [SerializeField] ParticleSystem _diveLandParticles;
    PlayerModel _model;
    public PlayerParticles(ParticleSystem newDive, ParticleSystem newDiveLand, PlayerModel model)
    {
        _diveParticles = newDive;
        _diveLandParticles = newDiveLand;
        _model = model;
    }

    public void FakeStart()
    {
        //EventManager.Trigger("OnDiveEnter");
        //EventManager.Trigger("OnGroundedEnter");
        //EventManager.Trigger("OnDiveEnd");

        EventManager.Subscribe("OnDiveEnter", PlayDiveParticles);
        EventManager.Subscribe("OnDiveEnd", StopDiveParticle);
        EventManager.Subscribe("OnDiveEnded", PlayDiveLandParticles);

    }

    public void PlayDiveParticles(params object[] noUse) 
    {
        PlayParticle(_diveParticles);

        //EventManager.Subscribe("OnGroundedEnter", PlayDiveLandParticles);

    }

    void StopDiveParticle(params object[] noUse)
    {
        StopParticle(_diveParticles);
    }

    public void PlayDiveLandParticles(params object[] noUse)
    {

        if (_model.CurrenState != PlayerState.Grounded) return;
        //StopParticle(_diveParticles);
        PlayParticle(_diveLandParticles);

        //Debug.Log("AHHHHHHHH de particulas");

        //EventManager.Unsubscribe("OnGroundedEnter", PlayDiveLandParticles);
    }

    public void PlayParticle(ParticleSystem parSystem)
    {
        parSystem.Play();
    }

    public void StopParticle(ParticleSystem parSystem)
    {
        parSystem.Stop();
    }

    public void FakeOnDestroy()
    {
        EventManager.Unsubscribe("OnDiveEnter", PlayDiveParticles);
        EventManager.Unsubscribe("OnDiveEnd", StopDiveParticle);
        EventManager.Unsubscribe("OnDiveEnded", PlayDiveLandParticles);
        EventManager.Unsubscribe("OnDiveEnded", PlayDiveLandParticles);
        EventManager.Unsubscribe("OnDiveEnded", PlayDiveLandParticles);
        //EventManager.Unsubscribe("OnGroundedEnter", PlayDiveLandParticles);
    }
}
