using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CheckpointModel
{
    Transform _pos;

    [SerializeField] bool _active = false;

    public event Action OnCheckpointActive = delegate { };

    public CheckpointModel(Transform pos)
    {
        _pos = pos;
    }

    public Vector3 GetPos()
    {

        if (!_active)
        {
            EventManager.Trigger("OnCheckpointActive");
            SetActive();

        }
        return _pos.position;
    }

    void SetActive()
    {
        Debug.Log("<color=green>Fogata Prendida</color>");


        _active = true;
        OnCheckpointActive();

        EventManager.Subscribe("OnCheckpointActive", SetInactive);

    }

    void SetInactive(params object[] noSeUsa)
    {
        Debug.Log("<color=red>Fogata Apagada</color>");

        _active = false;

        EventManager.Unsubscribe("OnCheckpointActive", SetInactive);
    }

    public void FakeOnDestroy()
    {
        EventManager.Unsubscribe("OnCheckpointActive", SetInactive);

    }
}
