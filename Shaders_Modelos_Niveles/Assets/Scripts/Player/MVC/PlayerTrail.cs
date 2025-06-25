using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrail : MonoBehaviour
{
    TrailRenderer _myTrail;
    [SerializeField] Transform _target;

    bool _active;
    public bool Active
    {
        get { return _active; }
        set { ChangeState(value); } 
    }
    private void Awake()
    {
        _myTrail = GetComponent<TrailRenderer>();
    }

    private void Start()
    {
        ChangeState(false);
    }

    private void Update()
    {
        MoveToTarget();
    }

    void MoveToTarget()
    {
        if (!_active) return;
        if(_target == null) return;

        transform.position = _target.position;
    }

    void ChangeState(bool state)
    {
        _active = state;
        transform.position = _target.position;


        if (state)
        {
            _myTrail.enabled = true;
        }
        else
        {
            _myTrail.enabled = false;
        }
    }
}
