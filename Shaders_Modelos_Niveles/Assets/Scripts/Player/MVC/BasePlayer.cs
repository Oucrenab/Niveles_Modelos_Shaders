using PlayerComplements;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : MonoBehaviour, IBounce, IDamageable, IMemento
{
    [SerializeField] PlayerModel _myModel;
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


    //Cosas para el trail
    [SerializeField] PlayerTrailFactory _trailFactory; //objectpoool
    SkinnedMeshRenderer[] _skMeshRenderer; //skinnedMeshRenderer
    [SerializeField] Material _trailShader; //mat
    [SerializeField] string _matAlphaName; //nombre variable del mat
    [SerializeField] PlayerTrail _trail;

    //cosas para la animacion
    [SerializeField] Animator _animator;
    [SerializeField] string _idle, _walk, _jump, _fall;
    [SerializeField] Transform _mesh;

    public Action<float> OnHorizontalInputChange = delegate { };
    public Action OnDieEnter = delegate { };
    public Action OnRespawn = delegate { };

    [SerializeField] float _deathDuration;
    Material _myMat;

    //cosas para el memento
    //MementoState _mementoState;

    private void Awake()
    {
        _myController = GetComponent<CharacterController>();
        _skMeshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();



        _myModel = new PlayerModel(this ,_myController, _speed, _baseFallSpeed, _jumpHeight, _jumpStr, _dashTime, _dashStr, _powerDashStr, _timeStopDuration, _deathDuration, new MementoState());
        _myControl = new PlayerControl(_myModel);

        _myModel.FakeAwake();
    }

    private void Start()
    {
        _myView = new PlayerView(_myModel, _trailFactory.Pool ,_skMeshRenderer, _trailShader , _matAlphaName, _dashTime, _animator, _idle, _walk, _jump, _fall, this, _mesh, _trail, _deathDuration);
        _myView.FakeStart();

        MementoSubscribe();
    }



    private void Update()
    {
        _myControl.FakeUpdate();

        _myModel.FakeUpdate();
        //movement


        _myView.FakeUpdate();


    }
    public void Bounce(Vector3 direction, float bounceStrg, float bounceDuration) => _myModel.Bounce(direction, bounceStrg, bounceDuration);

    public void GetDamage() => _myModel.GetDamage();

    private void OnDestroy()
    {
        _myView.FakeOnDestroy();
        _myModel.FakeDestroy();

        MementoUnsubscribe();
    }

    #region Memento
    public void Save(params object[] parameters) => _myModel.Save();

    public void Load(params object[] parameters) => _myModel.Load();

    public void MementoSubscribe()
    {
        EventManager.Subscribe("CallMementoLoad", Load);
        EventManager.Subscribe("CallMementoSave", Save);
    }

    public void MementoUnsubscribe()
    {
        EventManager.Unsubscribe("CallMementoLoad", Load);
        EventManager.Unsubscribe("CallMementoSave", Save);
    } 
    #endregion
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
