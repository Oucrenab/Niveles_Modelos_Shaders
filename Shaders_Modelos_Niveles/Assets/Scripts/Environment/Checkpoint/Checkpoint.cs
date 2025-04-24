using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour, ICheckpoint
{
    [SerializeField]CheckpointModel _model;
    CheckpointView _view;

    [SerializeField] Transform _pos;
    [SerializeField] ParticleSystem[] _fire;

    public Vector3 GetPosition() => _model.GetPos();


    private void Awake()
    {
        _model = new CheckpointModel(_pos);
        _view = new CheckpointView(_model, _fire);
    }

    private void OnDestroy()
    {
        _model.FakeOnDestroy();
        _view.FakeOnDestroy();
    }



}
