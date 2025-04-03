using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerComplements
{
    public class PlayerInputs
    {
        Player _myPlayer;

        //inputs
        int _dashInput = 0;
        int _timeStopInput = 1;
        int _diveInput = 2;
        [SerializeField] KeyCode _jumpKey = KeyCode.Space;
        [SerializeField] KeyCode _resetKey = KeyCode.R;
        [SerializeField] KeyCode _menuKey = KeyCode.Escape;
        [SerializeField] KeyCode _diveKey = KeyCode.LeftControl;


        public float lastHorizontal { get; private set; }
        public float lastVertical { get; private set; }

        public PlayerInputs(Player newPlayer)
        {
            _myPlayer = newPlayer;

            lastHorizontal = 0;
            lastVertical = 0;
        }

        public void FakeUpdate()
        {

            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");
                
            _myPlayer.PlayerMovement.SetAxisInput(horizontal, vertical);

            if (horizontal != 0)
                lastHorizontal = horizontal;
            
            if(vertical != 0)
                lastVertical = vertical;

            if (Input.GetMouseButtonDown(_dashInput))
            {
                //Debug.Log("<color=green>Dash</color>");
                _myPlayer.PlayerMovement.Dash();
            }
            if (Input.GetMouseButtonDown(_timeStopInput))
            {
                Debug.Log("<color=blue>Pausado</color>");
                _myPlayer.PlayerMovement.StartTimeStop();
            }
            if (Input.GetMouseButtonUp(_timeStopInput))
            {
                Debug.Log("<color=blue>Des-Pausado</color>");
                _myPlayer.PlayerMovement.EndTimeStop();
            }
            if (Input.GetKey(_jumpKey))
            {
                _myPlayer.PlayerMovement.Jump();
            }
            if(Input.GetKeyUp(_jumpKey))
            {
                _myPlayer.PlayerMovement.StopJump();
                _myPlayer.PlayerMovement.jumpKeyRelesad = true;
            }

            //if (Input.GetMouseButtonDown(_diveInput))//esto pal mouse
            if(Input.GetKeyDown(_diveKey))
            {
                _myPlayer.PlayerMovement.StartDive();
            }
        }
    }
}
