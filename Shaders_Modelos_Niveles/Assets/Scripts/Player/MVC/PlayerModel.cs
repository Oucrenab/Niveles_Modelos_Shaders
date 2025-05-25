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
    float _deathDuration;

    public Transform transform { get { return _player.transform; } }

    Vector3 _respawnPoint;

    event Action MovementUpdate = delegate { };

    //memento
    MementoState _mementoState;

    public PlayerModel
        (BasePlayer newPlayer,
        CharacterController newController,
        float newSpeed,
        float newFallSpeed,
        float newJumpHeight,
        float newJumpStr,
        float newDashTime,
        float newDashStr,
        float newPowerDashStr,
        float newTimestopDur,
        float deathDur,
        MementoState newMemento)
    {
        _player = newPlayer;
        _myController = newController;

        Speed = newSpeed;
        FallSpeed = newFallSpeed;
        JumpHeight = newJumpHeight;
        JumpStr = newJumpStr;
        DashTime = newDashTime;
        DashStr = newDashStr;
        PowerDashStr = newPowerDashStr;
        TimeStopDuration = newTimestopDur;
        _deathDuration = deathDur;

        _mementoState = newMemento;
    }


    public void FakeAwake()
    {
        CurrenState = PlayerState.Falling;
        _myMovement = new PlayerMovement(this, _myController, _player);
        MovementUpdate += _myMovement.FakeUpdate;

        _dashBehaviour = new PlayerDashBehaviour(this);
        _diveBehaviour = new PlayerDiveBehaviour(this);

        _checkpointCheck = new CheckpointCheck(this);

        _respawnPoint = new Vector3(0, 1, 0);

        //_myModel = this;
    }

    void PlayerStateChange(PlayerState newState)
    {

        var _lastState = _currentState;

        switch (newState)
        {
            case PlayerState.Grounded:
                EventManager.Trigger("OnGroundedEnter");

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

        if(_lastState == PlayerState.TimeStop && newState != PlayerState.TimeStop)
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

        CheckPlatformMovement();
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

        _player.OnRespawn();
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

    public void GetDamage()
    {
        MovementUpdate -= _myMovement.FakeUpdate;

        _player.OnDieEnter();
            
        StartCoroutine(Respawn(_deathDuration));
    } 
}
