using PlayerComplements;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerModel
{
    BasePlayer _player;
    CharacterController _myController;

    [SerializeField] PlayerMovement _myMovement;
    public PlayerMovement PlayerMovement { get { return _myMovement; } }

    PlayerDashBehaviour _dashBehaviour;
    PlayerDiveBehaviour _diveBehaviour;

    CheckpointCheck _checkpointCheck;
    PickeableCheck _pickeableCheck;

    [SerializeField] private PlayerState _currentState;
    public PlayerState CurrenState
    {
        get { return _currentState; }
        set { PlayerStateChange(value); }

    }

    //stats
    public float Speed;
    public float FallSpeed;
    public float JumpHeight;
    public float JumpStr;
    public float DashTime;
    public float DashStr;
    public float PowerDashStr;
    public float TimeStopDuration;
    public float TimeStopCD;
    float _deathDuration;

    public Transform transform { get { return _player.transform; } }

    Vector3 _respawnPoint;

    event Action MovementUpdate = delegate { };

    //memento
    MementoState _mementoState;

    //ScreenManager
    BaseScreen _pauseScreen;

    public PlayerModel
        (BasePlayer newPlayer,
        CharacterController newController,
        MementoState newMemento
        //float newSpeed,
        //float newFallSpeed,
        //float newJumpHeight,
        //float newJumpStr,
        //float newDashTime,
        //float newDashStr,
        //float newPowerDashStr,
        //float newTimestopDur,
        //float newTimestopCD,
        //float deathDur,
        //BaseScreen baseScreen
        )
    {
        _player = newPlayer;
        _myController = newController;
        _mementoState = newMemento;

        _pauseScreen = null;

        Speed = 1;
        FallSpeed = 1;
        JumpHeight = 1;
        JumpStr = 1;
        DashTime = 1;
        DashStr = 1;
        PowerDashStr = 1;
        TimeStopDuration = 1;
        TimeStopCD = 1;
        _deathDuration = 1;

    }


    public void FakeAwake()
    {
        CurrenState = PlayerState.Falling;
        _myMovement = new PlayerMovement(this, _myController, _player);
        MovementUpdate += _myMovement.FakeUpdate;

        _dashBehaviour = new PlayerDashBehaviour(this);
        _diveBehaviour = new PlayerDiveBehaviour(this);

        _checkpointCheck = new CheckpointCheck(this);
        _pickeableCheck = new PickeableCheck(this);

        _respawnPoint = new Vector3(0, 1, 0);

        //_myModel = this;
    }

    void PlayerStateChange(PlayerState newState)
    {
        if (_currentState == newState) return;

        var _lastState = _currentState;

        switch (newState)
        {
            case PlayerState.Grounded:
                EventManager.Trigger("OnGroundedEnter");
                //Debug.Log("EN el piso");
                break;
            case PlayerState.Walking:
                EventManager.Trigger("OnWalkEnter");

                break;
            case PlayerState.Jumping:
                EventManager.Trigger("OnJumpEnter");
                //Debug.Log("SALTOOOOOOOOOO");
                break;
            case PlayerState.Falling:
                EventManager.Trigger("OnFallEnter");

                break;
            case PlayerState.Dashing:
                //OnDashEnter();
                EventManager.Trigger("OnDashEnter");
                break;
            case PlayerState.Powerdashing:
                //OnPowerDashEnter();
                EventManager.Trigger("OnPowerDashEnter");

                break;
            case PlayerState.Diving:
                //OnDiveEnter();
                if(!_myController.isGrounded)
                EventManager.Trigger("OnDiveEnter");

                break;
            case PlayerState.Bouncing:
                //OnBounceEnter();
                EventManager.Trigger("OnBounceEnter");

                break;
            case PlayerState.TimeStop:
                //OnTimeStopEnter();
                EventManager.Trigger("OnTimeStopEnter");

                break;
        }

        if (_lastState == PlayerState.Dashing && newState != PlayerState.Dashing)
        {
            //OnDashEnd();
            EventManager.Trigger("OnDashEnd");
        }

        if (_lastState == PlayerState.Diving && newState != PlayerState.Diving)
        {
            //OnDashEnd();
            EventManager.Trigger("OnDiveEnd");
        }

        if (_lastState == PlayerState.TimeStop && newState != PlayerState.TimeStop)
            EventManager.Trigger("OnTimeStopEnd");



        _currentState = newState;
    }

    public void StartCoroutine(IEnumerator rutine)
    {
        //Debug.Log($"{rutine}");
        _player.StartCoroutine(rutine);
    }

    public void FakeUpdate()
    {
        //Debug.Log($"<color=yellow>Update de Model</color>");

        //Debug.Log($"{CurrenState}");

        //_myMovement.FakeUpdate();
        MovementUpdate();

        _dashBehaviour.FakeUpdate();
        _diveBehaviour.FakeUpdate();

        _checkpointCheck.FakeUpdate();
        _pickeableCheck.FakeUpdate();

        CheckPlatformMovement();
    }

    public void FakeDestroy()
    {
        _myMovement.FakeDestroy();
    }

    public void Bounce(Vector3 direction, float bounceStrg, float bounceDuration)
    {

        StartCoroutine(_myMovement.BounceRoutine(direction, bounceStrg, bounceDuration));
        _myMovement.RefreshAllMovement();

        //StartCoroutine(BounceRoutine(direction, bounceStrg, bounceDuration));
        //RefreshAllMovement();
    }

    void CheckPlatformMovement()
    {
        if (Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), Vector3.down, out var hit, 0.3f)
            && hit.transform.TryGetComponent<IMovingPlatform>(out var platform))
        {
            _myMovement.CopyMovement(platform.GetMovement() * Time.deltaTime);
            //CopyMovement(platform.GetMovement() * Time.deltaTime);
            //_currentState = PlayerState.Grounded;
            _myMovement.RefreshAllMovement();
        }
    }

    public void SetRespawnPoint(Vector3 pos)
    {
        //Debug.Log($"SpawnSeteado en {pos}");
        _respawnPoint = pos;
    }

    IEnumerator Respawn(float wait)
    {
        yield return new WaitForSeconds(wait);

        //Debug.Log($"Volviendo a {_respawnPoint}");

        MovementUpdate += _myMovement.FakeUpdate;


        //_myController.enabled = false;
        //_player.transform.position = _respawnPoint;
        //_myController.enabled = true;

        EventManager.Trigger("CallMementoLoad");

        //_player.OnRespawn();
        EventManager.Trigger("OnRespawn");

    }

    public void Save()
    {
        Debug.Log("Player Guardado");
        _mementoState.Rec(_player.transform.position);
    }

    public void Load()
    {
        if (!_mementoState.IsRemember()) return;
        Debug.Log("Player Cargado");

        var remember = _mementoState.Remember();

        _myController.enabled = false;
        _player.transform.position = (Vector3)remember.parameters[0];
        _myController.enabled = true;
    }

    public void GetDamage(DamageTipe tipe)
    {
        MovementUpdate -= _myMovement.FakeUpdate;

        //_player.OnDieEnter();
        EventManager.Trigger("DamageEnter", tipe);
        EventManager.Trigger("OnDieEnter");

        StartCoroutine(Respawn(_deathDuration));
    } 

    public void ActivatePause()
    {
        if (_pauseScreen == null) return;

        if (ScreenManager.Instance.ScreenActive)
            ScreenManager.Instance.DeactivateScreen();
        else
            ScreenManager.Instance.ActivateScreen(_pauseScreen);
    }

    #region Builder

    public PlayerModel SetSpeeds(float speed, float fallSpeed)
    {
        Speed = speed;
        FallSpeed = fallSpeed;

        return this;
    }

    public PlayerModel SetJumpData(float jumpHeight, float jumpStr)
    {
        JumpHeight = jumpHeight;
        JumpStr = jumpStr;

        return this;
    }

    public PlayerModel SetDashData(float dashTime, float dashStr, float powerDashStr)
    {
        DashTime = dashTime;
        DashStr = dashStr;
        PowerDashStr = powerDashStr;

        return this;
    }

    public PlayerModel SetTimeStopData(float timeStopDuration, float timeStopCD)
    {
        TimeStopDuration = timeStopDuration;
        TimeStopCD = timeStopCD;

        return this;
    }

    public PlayerModel SetPauseScreen(BaseScreen pause)
    {
        _pauseScreen = pause;

        return this;
    }


    #endregion
}
