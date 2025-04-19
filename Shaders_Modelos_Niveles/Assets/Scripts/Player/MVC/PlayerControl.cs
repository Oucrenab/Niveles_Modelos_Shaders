using PlayerComplements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl
{
    Player _myPlayer;
    PlayerModel _myModel;
    PlayerMovement _myMovement;

    //inputs
    int _dashInput = 0;
    int _timeStopInput = 1;
    int _diveInput = 2;
    [SerializeField] KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] KeyCode _resetKey = KeyCode.R;
    [SerializeField] KeyCode _menuKey = KeyCode.Escape;
    [SerializeField] KeyCode _diveKey = KeyCode.LeftControl;

    public PlayerControl(PlayerModel model)
    {
        //_myPlayer = newPlayer;
        _myModel = model;
        //_myMovement = movement;
    }

    public void FakeUpdate()
    {
        //Debug.Log($"<color=green>Update de control</color>");

        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        _myModel.PlayerMovement.SetAxisInput(horizontal, vertical);

        if (Input.GetMouseButtonDown(_dashInput))
        {
            //Debug.Log("<color=green>Dash</color>");
            //_myPlayer.PlayerMovement.Dash();
            _myModel.PlayerMovement.Dash();
            //_myMovement.Dash();
        }
        if (Input.GetMouseButtonDown(_timeStopInput))
        {
            //Debug.Log("<color=blue>Pausado</color>");
            //_myPlayer.PlayerMovement.StartTimeStop();
            _myModel.PlayerMovement.StartTimeStop();
        }
        if (Input.GetMouseButtonUp(_timeStopInput))
        {
            //Debug.Log("<color=blue>Des-Pausado</color>");
            //_myPlayer.PlayerMovement.EndTimeStop();
            _myModel.PlayerMovement.EndTimeStop();
        }
        if (Input.GetKey(_jumpKey))
        {
            //_myPlayer.PlayerMovement.Jump();
            _myModel.PlayerMovement.Jump();
            //_myMovement.Jump();
        }
        if (Input.GetKeyUp(_jumpKey))
        {
            //_myPlayer.PlayerMovement.StopJump();
            _myModel.PlayerMovement.StopJump();
            //_myMovement.StopJump();
            //_myPlayer.PlayerMovement.jumpKeyRelesad = true;
            _myModel.PlayerMovement.jumpKeyRelesad = true;
            //_myMovement.jumpKeyRelesad = true;
        }

        //if (Input.GetMouseButtonDown(_diveInput))//esto pal mouse
        if (Input.GetKeyDown(_diveKey))
        {
            //_myPlayer.PlayerMovement.StartDive();
            _myModel.PlayerMovement.StartDive();
            //_myMovement.StartDive();
        }
    }
}
