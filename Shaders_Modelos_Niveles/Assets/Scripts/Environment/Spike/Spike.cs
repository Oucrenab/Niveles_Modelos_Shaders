using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    SpikeModel _model;
    SpikeView _view;
    [SerializeField] DamageTipe _damageType;

    private void Awake()
    {
        _model = new SpikeModel( _damageType);
        _view = new SpikeView();
    }

    void Update()
    {
        _model.FakeUpdate();
        _view.FakeUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"Spike Trigger {other.name}");
        _model.FakeOnTriggerEnter(other);
    }
}
