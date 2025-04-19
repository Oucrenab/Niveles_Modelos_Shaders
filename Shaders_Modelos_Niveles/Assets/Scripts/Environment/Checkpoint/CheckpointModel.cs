using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointModel
{
    Transform _pos;

    bool _active = false;

    public event Action OnCheckpointActive = delegate { };

    public CheckpointModel(Transform pos)
    {
        _pos = pos;
    }

    public Vector3 GetPos()
    {

        if (!_active)
        {
            _active = true;
            OnCheckpointActive();
        }
        return _pos.position;
    }
}
