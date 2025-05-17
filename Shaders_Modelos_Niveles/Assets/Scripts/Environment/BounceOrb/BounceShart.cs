using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class BounceShart : MonoBehaviour, IDiveable, IDasheable
{
    [SerializeField] BounceShartModel _myModel;
    [Space]
    [Header("BounceData")]
    [SerializeField] Transform _targetDir;
    [SerializeField] float _bounceDuration;
    [Space]
    [SerializeField] float _diveForce;
    [SerializeField] float _diveDuration;

    public event Action OnDashed = delegate { };
    public event Action OnDived = delegate { };


    private void Awake()
    {
        var collider = GetComponent<Collider>();

        _myModel = new BounceShartModel(transform, _targetDir, _bounceDuration, collider, this, _diveForce, _diveDuration);
        //_myView = new BounceRockView(this, transform.GetComponent<Renderer>().material, _maxDeformScale, _bounceDuration);
    }

    public void Dashed(Transform a)
    {
        _myModel.Dashed(a);

        OnDashed();

    }

    public void Dived(Transform a)
    {
        _myModel.Dived();

        OnDived();
    }

    public void CallCoroutine(IEnumerator routine)
    {
        StartCoroutine(routine);
    }
}
