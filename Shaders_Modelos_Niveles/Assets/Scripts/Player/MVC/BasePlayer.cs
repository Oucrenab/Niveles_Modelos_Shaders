using PlayerComplements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : MonoBehaviour, IBounce, IDamageable
{
    PlayerModel _myModel;
    PlayerControl _myControl;
    PlayerView _myView;

    CharacterController _myController;

    [Header("<color=green>Player Stats</color>")]
    [SerializeField, Range(0, 20)] float _speed;
    [SerializeField, Range(0.1f, 20)] float _baseFallSpeed;
    [SerializeField, Range(0, 5)] float _jumpHeight;
    [SerializeField, Range(0, 100)] float _jumpStr;

    [SerializeField, Range(0, 2)] float _dashTime;
    [SerializeField] float _dashStr;
    [SerializeField] float _powerDashStr;

    [SerializeField, Range(0, 1)] float _timeStopDuration;

    public static Transform PlayerTransform;

    private void Awake()
    {
        PlayerTransform = transform;
        _myController = GetComponent<CharacterController>();

        _myModel = new PlayerModel(this ,_myController, _speed, _baseFallSpeed, _jumpHeight, _jumpStr, _dashTime, _dashStr, _powerDashStr, _timeStopDuration);
        _myControl = new PlayerControl(_myModel);
        _myView = new PlayerView(_myModel);

        _myModel.FakeAwake();
    }

    public void Bounce(Vector3 direction, float bounceStrg, float bounceDuration)
    {

        _myModel.Bounce(direction, bounceStrg, bounceDuration);
    }

    private void Update()
    {
        _myControl.FakeUpdate();

        _myModel.FakeUpdate();
        //movement


        _myView.FakeUpdate();


    }

    public void GetDamage()
    {
        _myModel.GetDamage();
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
