using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour, ICheckpoint
{
    CheckpointModel _model;
    CheckpointView _view;

    [SerializeField] Transform _pos;
    [SerializeField] GameObject _fire;

    public Vector3 GetPosition() => _model.GetPos();


    private void Awake()
    {
        _model = new CheckpointModel(_pos);
        _view = new CheckpointView(_model, _fire);
    }





}
