using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointView
{
    ParticleSystem[] _fire;
    CheckpointModel _model;

    public CheckpointView(CheckpointModel model, ParticleSystem[] fire)
    {
        _model = model;
        _fire = fire;

        _model.OnCheckpointActive += TurnOn;

        EventManager.Subscribe("OnCheckpointActive", TurnOff);
    }

    public void TurnOn()
    {
        Debug.Log("<color=green>Prendido</color>");
        foreach (var item in _fire)
        {
            item.Play();
        }
    }

    public void TurnOff(params object[] noSeUsa)
    {
        Debug.Log("<color=red>Apagado</color>");
        foreach (var item in _fire)
        {
            item.Stop();
        }
    }

    public void FakeOnDestroy()
    {
        EventManager.Unsubscribe("OnCheckpointActive", TurnOff);

    }
}
