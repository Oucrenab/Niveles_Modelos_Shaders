using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView
{
    PlayerModel _myModel;

    public PlayerView(PlayerModel model) 
    {  
        _myModel = model; 
    }

    public void FakeUpdate()
    {
        //Debug.Log($"<color=red>Update de View</color>");

    }
}
