using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerComplements
{
    public class PlayerMovement
    {
        Player _myPlayer;
        CharacterController _myController;

        float _gravity = 9.81f;
        float _verticalVelocity;
        float _horizontalInput;
        float _verticalInput;
        float _gravityMul = 1;

        bool _canJump = true;
        bool _canDash = false;
        bool _canMove = true;
        bool _canDive = true;
        bool _canTimeStop = true;

        private delegate void VoidDelegate();
        private VoidDelegate Movemet = delegate { };


        public PlayerMovement(Player newPlayer, CharacterController newController)
        {
            _myPlayer = newPlayer;
            _myController = newController;

            Movemet += VerticalForceCalculation;
            Movemet += Walk;
            Movemet += PlayerGravity;

        }

        public void FakeUpdate()
        {
            Movemet();

            if(_myPlayer.CurrenState == PlayerState.Falling)
                FastFallingCalc();
                

            //Debug.Log($"Velocidad vertical = {_verticalVelocity}");
        }



        public void Walk()
        {
            if (!_canMove) return;
            if(_myPlayer.CurrenState == PlayerState.Diving) return;
            if(_myPlayer.CurrenState == PlayerState.TimeStop) return;


            var dir = new Vector3(_horizontalInput, 0, 0) * _myPlayer.Speed;
            //dir.y = _verticalVelocity;
            

            _myController.Move(dir * Time.deltaTime);   
        }

        public void SetAxisInput(float newHorizontal, float newVertical)
        {
            _horizontalInput = newHorizontal;
            _verticalInput = newVertical;

        }

        public IEnumerator BounceRoutine(Vector3 dir, float strg, float duration)
        {
            _myPlayer.CurrenState = PlayerState.Bouncing;

            float startTime = Time.time;

            //reset salto
            //, dash
            //, dive

            while (_myPlayer.CurrenState == PlayerState.Bouncing && Time.time < startTime + duration)
            {
                _myController.Move(dir * strg * Time.deltaTime);

                yield return null;
            }

            if (_myPlayer.CurrenState == PlayerState.Bouncing)
                _myPlayer.CurrenState = PlayerState.Falling;
        }

        #region Jump Logic
        float _initialHeight = -100;
        bool _jumpStarted = false;
        public bool jumpKeyRelesad = true;

        public void Jump()
        {
            if (!_canJump) return;
            if (!jumpKeyRelesad) return;
            Debug.Log("<color=yellow> Saltando </color>");
            //_verticalVelocity = Mathf.Sqrt(_myPlayer.JumpHeight * _gravity * 2f); //salto instantaneo a alura determinada

            if (!_jumpStarted)
                StartJump();

            if (_myPlayer.transform.position.y > _initialHeight + _myPlayer.JumpHeight)
                StopJump();

            _verticalVelocity = 10 * _myPlayer.JumpStr * Time.fixedDeltaTime;


        }

        void StartJump()
        {
            Debug.Log("<color=green> SAlto Iniciado </color>");


            _jumpStarted = true;
            _initialHeight = _myPlayer.transform.position.y;

            _myPlayer.CurrenState = PlayerState.Jumping;
        }

        public void StopJump()
        {
            if(!_jumpStarted) return;
            Debug.Log("<color=red> Salto terminado </color>");

            _canJump = false;
            _jumpStarted = false;
            _initialHeight = -100;
            jumpKeyRelesad = false;

            if (_myPlayer.CurrenState == PlayerState.Jumping)
                _myPlayer.CurrenState = PlayerState.Falling;
        } 

        public void RefreshJump()
        {
            _canJump = true;
        }
        #endregion

        #region Dash Logic
        public void Dash()
        {
            if (!_canDash) return;
            //_verticalVelocity = Mathf.Sqrt(_myPlayer.JumpHeight * _gravity * 2f);
            //var dir = new Vector3(0, 0, 0);

            //if (_horizontalInput == 0 && _verticalInput == 0)
            //{
            //    dir = new Vector3(_myPlayer.PlayerInputs.lastHorizontal, 0, 0);
            //}
            //else
            //    dir = new Vector3(_horizontalInput, _verticalInput, 0).normalized;

            //var dashMult = Mathf.Sqrt(_myPlayer.DashTime * _myPlayer.DashStr * 2f); 

            //_myController.Move(dir * dashMult);

            switch (_myPlayer.CurrenState)
            {
                case PlayerState.Jumping:
                    StopJump();
                    break;
                case PlayerState.Diving:
                    return;
            }

            _myPlayer.StartCoroutine(DashRoutine());
        }

        IEnumerator DashRoutine()
        {
            _canDash = false;
            float dashStrg;

            if(_myPlayer.CurrenState == PlayerState.TimeStop)
            {
                EndTimeStop();
                _myPlayer.CurrenState = PlayerState.Powerdashing;
                dashStrg = _myPlayer.PowerDashStr;
            }
            else
            {
                _myPlayer.CurrenState = PlayerState.Dashing;
                dashStrg = _myPlayer.DashStr;
            }


            float startTime = Time.time;

            var dir = new Vector3(0, 0, 0);

            if (_horizontalInput == 0 && _verticalInput == 0)
            {
                dir = new Vector3(_myPlayer.PlayerInputs.lastHorizontal, 0, 0);
            }
            else
                dir = new Vector3(_horizontalInput, _verticalInput, 0).normalized;


            while ((_myPlayer.CurrenState == PlayerState.Dashing || _myPlayer.CurrenState == PlayerState.Powerdashing)
                && Time.time < startTime + _myPlayer.DashTime)
            {
                _myController.Move(dir * dashStrg * Time.deltaTime);

                yield return null;
            }

            if (_myPlayer.CurrenState == PlayerState.Dashing || _myPlayer.CurrenState == PlayerState.Powerdashing)
                EndDash();
        }

        void EndDash()
        {
            _myPlayer.CurrenState = PlayerState.Falling;
        }

        public void RefreshDash()
        {
            _canDash = true;
        } 
        #endregion

        #region Dive Logic
        public void StartDive()
        {
            if (!_canDive) return;

            if (_myPlayer.CurrenState == PlayerState.Grounded || _myPlayer.CurrenState == PlayerState.Diving)
            {
                return;
            }

            _myPlayer.StartCoroutine(DiveRoutine());

        }

        IEnumerator DiveRoutine()
        {
            _myPlayer.CurrenState = PlayerState.Diving;

            var dir = new Vector3(0, -1, 0);

            while (_myPlayer.CurrenState == PlayerState.Diving)
            {
                _myController.Move(dir * _gravity * 5f * Time.deltaTime);

                yield return null;
            }
        }

        void EndDive()
        {
            _myPlayer.CurrenState = PlayerState.Grounded;

            RefreshDive();
        }

        public void RefreshDive()
        {
            _canDive = true;
        }
        #endregion

        #region Timestop Logic
        public void StartTimeStop()
        {
            if(!_canTimeStop) return;

            if(_myPlayer.CurrenState == PlayerState.TimeStop) return;
            if(_myPlayer.CurrenState == PlayerState.Diving) return;

            StopJump();
            RefreshDash();

            _myPlayer.StartCoroutine(TimeStopCoroutine());
        }

        IEnumerator TimeStopCoroutine()
        {
            _canTimeStop = false;
            _myPlayer.CurrenState = PlayerState.TimeStop;

            float startTime = Time.time;


            //while (_myPlayer.CurrenState == PlayerState.TimeStop && Time.time < startTime + _myPlayer.TimeStopDuration)
            //{


            //    yield return null;
            //}
            Time.timeScale = 0.1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;

            yield return new WaitForSecondsRealtime(_myPlayer.TimeStopDuration);

            if(_myPlayer.CurrenState == PlayerState.TimeStop)
            {
                EndTimeStop();
            }
        }

        public void EndTimeStop()
        {
            if (_myPlayer.CurrenState != PlayerState.TimeStop) return;

            if (_myPlayer.CurrenState != PlayerState.Powerdashing)
                _myPlayer.CurrenState = PlayerState.Grounded;

            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
            //restaurar tiempo
        }

        public void RefreshTimeStop()
        {
            _canTimeStop = true;

        }
        #endregion

        #region Gravity Logic
        void FastFallingCalc()
        {
            _gravityMul += Time.deltaTime * 2f;

            if (_gravityMul > 5)
                _gravityMul = 5;
        }

        void VerticalForceCalculation()
        {

            switch (_myPlayer.CurrenState)
            {
                case PlayerState.Grounded:
                case PlayerState.Walking:
                    _verticalVelocity = -1f;
                    break;
                case PlayerState.Jumping:
                case PlayerState.Diving:
                    //Velocidad Vertical asiganda en otro lado, boludito

                    break;
                case PlayerState.Falling:
                    //Gravedad contante
                    _verticalVelocity -= _myPlayer.FallSpeed * _gravity * _gravityMul * Time.deltaTime;
                    break;
                case PlayerState.Dashing:
                case PlayerState.Powerdashing:
                case PlayerState.TimeStop:
                case PlayerState.Bouncing:
                    //ignora gravedad
                    _verticalVelocity = 0f;
                    break;
            }

            //if (_myController.isGrounded && !_jumpStarted)
            //{
            //    _myPlayer.CurrenState = PlayerState.Grounded;
            //    _canJump = true;
            //    _gravityMul = 1;
            //}

            //if (_myController.isGrounded && !_canDash)
            //{
            //    RefreshDash();
            //}

            //if (_myController.isGrounded && _myPlayer.CurrenState == PlayerState.Diving)
            //{
            //    EndDive();
            //}

            if (_myController.isGrounded)
            {
                if (!_jumpStarted)
                {
                    //_myPlayer.CurrenState = PlayerState.Grounded;
                    _canJump = true;
                    _gravityMul = 1;
                }

                if (!_canDash)
                {
                    RefreshDash();
                }

                if (_myPlayer.CurrenState == PlayerState.Diving)
                {
                    EndDive();
                }

                if (!_canTimeStop)
                {
                    RefreshTimeStop();
                }

                if(_myPlayer.CurrenState != PlayerState.TimeStop)
                    _myPlayer.CurrenState = PlayerState.Grounded;
            }

            if (!_myController.isGrounded
                && _myPlayer.CurrenState != PlayerState.Jumping
                && _myPlayer.CurrenState != PlayerState.Diving
                && _myPlayer.CurrenState != PlayerState.Bouncing
                && _myPlayer.CurrenState != PlayerState.Dashing
                && _myPlayer.CurrenState != PlayerState.Powerdashing
                && _myPlayer.CurrenState != PlayerState.TimeStop)
            {
                _myPlayer.CurrenState = PlayerState.Falling;
            }


            //if (_myController.isGrounded)
            //{
            //    _verticalVelocity = -1f;
            //    _canJump = true;

            //    if(_myPlayer.currenState != PlayerState.Grounded)
            //        _myPlayer.currenState = PlayerState.Grounded;
            //}
            //else
            //{
            //     _verticalVelocity-= _gravity * Time.deltaTime;
            //}

            //return _verticalVelocity;
        }

        public void PlayerGravity()
        {
            var dir = new Vector3(0, _verticalVelocity, 0);
            _myController.Move(dir * Time.deltaTime);
        } 
        #endregion

        public void CopyMovement(Vector3 movement)
        {
            _myController.Move(movement);
        }

        public void RefreshAllMovement()
        {
            RefreshDash();
            RefreshDive();
            RefreshJump();
            RefreshTimeStop();
        }

    }
}