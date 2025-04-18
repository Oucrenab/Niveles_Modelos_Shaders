using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointCheck
{
    PlayerModel _myModel;

    Vector3 _lastCheckpoint;

    public CheckpointCheck(PlayerModel myModel)
    {
        _myModel = myModel;
    }

    void FakeUpdate()
    {
        Check();
    }

    void Check()
    {
        var offset = new Vector3(0,1,0);
        var other = Physics.OverlapSphere(_myModel.transform.position + offset, 1f);

        Vector3 pos = Vector3.zero;

        foreach (var item in other)
        {
            if(item.TryGetComponent<ICheckpoint>(out var checkpoint))
            {
                pos = checkpoint.GetPosition();
            }
        }

        if(pos != Vector3.zero && pos != _lastCheckpoint)
        {
            ChangeCheckpoint(pos);
        }
    }

    void ChangeCheckpoint(Vector3 pos)
    {
        _lastCheckpoint = pos;
        _myModel.SetRespawnPoint(_lastCheckpoint);
    }
}
