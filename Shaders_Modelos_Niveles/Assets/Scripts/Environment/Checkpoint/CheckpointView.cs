using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointView
{
    GameObject _fire;
    CheckpointModel _model;

    public CheckpointView(CheckpointModel model, GameObject fire)
    {
        _model = model;
        _fire = fire;

        _model.OnCheckpointActive += TurnOn;
    }

    public void TurnOn()
    {
        _fire.SetActive(true);
    }

    public void TurnOff()
    {
        _fire.SetActive(false);
    }
}
