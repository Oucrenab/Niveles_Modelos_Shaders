using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Estalactita : MonoBehaviour, IMemento
{
    //Ray cast para detectar player
    //si detecta cae hasta x pos
    //Activar shader, particulas

    [SerializeField] Transform _fallingObj;
    [SerializeField] Transform _endPos;
    [SerializeField] float _fallTime;
    [SerializeField] ParticleSystem _drops;
    [SerializeField] ParticleSystem _rocks;
    [SerializeField] EstalactitaDamage _damageObj;

    float _speed;
    Vector3 _movementDir;
    MementoState _mementoState;

    bool _active = true;

    Action MovementAction;

    private void Start()
    {
        _mementoState = new MementoState();
        MementoSubscribe();
    }

    private void OnDestroy()
    {
        MementoUnsubscribe();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_active) return;

        if (other.GetComponent<IDamageable>() != null)
            TriggerFall();
    }


    private void Update()
    {
        MovementAction?.Invoke();
    }
    void MovementUpdate()
    {
        if (Vector3.Distance(_endPos.position, _fallingObj.position) < 0.5f)
        {
            MovementAction -= MovementUpdate;
            _fallingObj.position = _endPos.position;
            _damageObj.SetActive(false);
            return;
        }

        Movement();
    }

    void TriggerFall()
    {
        StartMovement();
        _active = false;
        _damageObj.SetActive(true);

        MovementAction += MovementUpdate;
    }

    void StartMovement()
    {
        _speed = GetSpeed(GetDir(_endPos, _fallingObj), _fallTime);
    }

    private void Movement()
    {
        if (_speed <= 0) return;
        _movementDir = GetDir(_endPos, _fallingObj);

        _fallingObj.position += _movementDir.normalized * Time.deltaTime * _speed;
    }

    Vector3 GetDir(Transform finalPos, Transform currentPos)
    {
        var dir = finalPos.position - currentPos.position;

        return dir;
    }

    float GetSpeed(Vector3 dir, float pathTime)
    {
        var speed = dir.magnitude / pathTime;

        return speed;
    }

    public void Save()
    {
        _mementoState.Rec(_fallingObj.position, _active, _speed);
    }

    public void Load()
    {
        if (!_mementoState.IsRemember()) return;

        var remember = _mementoState.Remember();

        _fallingObj.position = (Vector3)remember.parameters[0];
        _active = (bool)remember.parameters[1];
        _speed = (float)remember.parameters[2];

    }

    #region Memento

    public void Load(params object[] parameters) => Load();

    public void Save(params object[] parameters) => Save();

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
