using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerComplements;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour, IBounce
{


    PlayerInputs _myInputs;
    PlayerMovement _myMovement;
    PlayerDashBehaviour _myDashBehaviour;
    PlayerDiveBehaviour _myDiveBehaviour;

    public PlayerMovement PlayerMovement { get { return _myMovement; } }
    public PlayerInputs PlayerInputs { get { return _myInputs; } }

    CharacterController _myController;

    [Header("<color=green>Player Stats</color>")]
    [SerializeField, Range(0, 20)] float _speed;
    [SerializeField, Range(0.1f, 20)] float _baseFallSpeed;
    [SerializeField, Range(0,5)] float _jumpHeight;
    [SerializeField, Range(0,100)] float _jumpStr;

    [SerializeField, Range(0,2)] float _dashTime;
    [SerializeField] float _dashStr;
    [SerializeField] float _powerDashStr;

    [SerializeField, Range(0, 1)] float _timeStopDuration;

    public float Speed { get { return _speed; } }
    public float FallSpeed {  get { return _baseFallSpeed; } }
    public float JumpHeight { get { return _jumpHeight; } }
    public float JumpStr { get { return _jumpStr; } }
    public float DashTime { get { return _dashTime; } }
    public float DashStr { get { return _dashStr; } }
    public float PowerDashStr { get { return _powerDashStr; } }
    public float TimeStopDuration {  get { return _timeStopDuration; } }


    [SerializeField] private PlayerState _currentState;
    public PlayerState CurrenState
    {
        get { return _currentState; }
        set { PlayerStateChange(value); }
    }


    public delegate void VoidDelegate();

    VoidDelegate FakeUpdates = delegate { };

    private void Awake()
    {
        _myController = GetComponent<CharacterController>();

        _myInputs = new PlayerInputs(this);
        //_myMovement = new PlayerMovement(this, _myController);
        //_myDashBehaviour = new PlayerDashBehaviour(this);
        //_myDiveBehaviour = new PlayerDiveBehaviour(this);

        FakeUpdates += _myInputs.FakeUpdate; 
        FakeUpdates += _myMovement.FakeUpdate;
        FakeUpdates += _myDashBehaviour.FakeUpdate; 
        FakeUpdates += _myDiveBehaviour.FakeUpdate;

    }

    private void Update()
    {
        FakeUpdates();

        CheckPlatformMovement();
    }
    
    public void Bounce(Vector3 direction, float bounceStrg, float bounceDuration)
    {

        StartCoroutine(_myMovement.BounceRoutine(direction, bounceStrg, bounceDuration));
        _myMovement.RefreshAllMovement();
    }

    void CheckPlatformMovement()
    {
        if(Physics.Raycast(transform.position + new Vector3(0,0.1f,0), Vector3.down, out var hit, 0.3f) 
            && hit.transform.TryGetComponent<IMovingPlatform>(out var platform))
        {
            _myMovement.CopyMovement(platform.GetMovement()*Time.deltaTime);
            
        }
    }

    void PlayerStateChange(PlayerState newState)
    {

        var _lastState = _currentState;

        switch(newState)
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

        if(_lastState == PlayerState.Dashing && newState != PlayerState.Dashing)
        {
            //OnDashEnd();
            EventManager.Trigger("OnDashEnd");
        }


        _currentState = newState;
    }
    
}

public enum PlayerState
{
    Grounded,
    Walking,
    Jumping,
    Falling,
    Dashing,
    Powerdashing,
    Diving,
    Bouncing,
    TimeStop

}
