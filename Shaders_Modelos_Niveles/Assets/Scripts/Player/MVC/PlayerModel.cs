using PlayerComplements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel
{
    BasePlayer _player;
    CharacterController _myController;

    PlayerMovement _myMovement;
    public PlayerMovement PlayerMovement { get { return _myMovement; } }

    PlayerDashBehaviour _dashBehaviour;
    PlayerDiveBehaviour _diveBehaviour;

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

    public Transform transform { get { return _player.transform; } }

    Vector3 _respawnPoint;

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
        float newTimestopDur)
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

        _myMovement = new PlayerMovement(this, _myController);

        _dashBehaviour = new PlayerDashBehaviour(this);
        _diveBehaviour = new PlayerDiveBehaviour(this);

        _respawnPoint = new Vector3(0,1,0);
    }

    void PlayerStateChange(PlayerState newState)
    {

        var _lastState = _currentState;

        switch (newState)
        {
            case PlayerState.Grounded:
                break;
            case PlayerState.Walking:
                break;
            case PlayerState.Jumping:
                break;
            case PlayerState.Falling:
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


        _currentState = newState;
    }

    public void StartCoroutine(IEnumerator rutine)
    {
        _player.StartCoroutine(rutine);
    }

    public void FakeUpdate()
    {
        Debug.Log($"<color=yellow>Update de Model</color>");

        Debug.Log($"{CurrenState}");

        _myMovement.FakeUpdate();

        _dashBehaviour.FakeUpdate();
        _diveBehaviour.FakeUpdate();

        CheckPlatformMovement();
    }

    public void Bounce(Vector3 direction, float bounceStrg, float bounceDuration)
    {

        StartCoroutine(_myMovement.BounceRoutine(direction, bounceStrg, bounceDuration));
        _myMovement.RefreshAllMovement();
    }

    void CheckPlatformMovement()
    {
        if (Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), Vector3.down, out var hit, 0.3f)
            && hit.transform.TryGetComponent<IMovingPlatform>(out var platform))
        {
            _myMovement.CopyMovement(platform.GetMovement() * Time.deltaTime);

        }
    }

    public void SetRespawnPoint(Vector3 pos)
    {
        _respawnPoint = pos;
    }

    public void Respawn()
    {
        //transform.position = _respawnPoint;
    }

    public void GetDamage()
    {
        Respawn();
    }
    
}
