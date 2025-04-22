using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour, IMovingPlatform
{
    [SerializeField] MovingPlatformModel _myModel;

    [SerializeField] Transform[] _wayPoints;
    [SerializeField] float[] _timeBtPoints;

    public Vector3 GetMovement() => _myModel.GetMovement();

    private void Awake()
    {
        _myModel = new MovingPlatformModel(_wayPoints, _timeBtPoints, transform);
    }

    private void Update()
    {
        _myModel.FakeUpdate();
    }
}
