using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickeableCheck
{
    PlayerModel _myModel;

    public PickeableCheck(PlayerModel myModel)
    {
        _myModel = myModel;
    }

    public void FakeUpdate()
    {
        Check();
    }

    void Check()
    {
        var offset = new Vector3(0, 1, 0);
        var other = Physics.OverlapSphere(_myModel.transform.position + offset, 1.1f);

        foreach (var item in other)
        {
            if (item.TryGetComponent<IPickeable>(out var checkpoint))
            {
                checkpoint.PickUp();
                //EventManager.Trigger("CallMementoSave");
            }
        }
    }
}
