using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    SpikeModel _model;
    SpikeView _view;

    private void Awake()
    {
        _model = new SpikeModel();
        _view = new SpikeView();
    }

    void Update()
    {
        _model.FakeUpdate();
        _view.FakeUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        _model.FakeOnTriggerEnter(other);
    }
}
