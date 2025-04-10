using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour, IMovingPlatform
{
    [SerializeField] Transform[] _pathPoints;
    //[SerializeField] float _speed;
    [SerializeField] float[] _timeBtPoints;
    int _currentPointIndex;
    float _speed;
    Vector3 _movementDir;


    private void Update()
    {
        if (Vector3.Distance(_pathPoints[_currentPointIndex].position, transform.position) < 0.5f)
            NextPoint();

        Movement();
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

    void StopMovement()
    {
        _speed = 0;
    }

    void StartMovement()
    {
        _speed = GetSpeed(GetDir(_pathPoints[_currentPointIndex], transform), _timeBtPoints[_currentPointIndex]);
    }

    void NextPoint()
    {
        _currentPointIndex++;

        if (_currentPointIndex >= _pathPoints.Length)
        {
            _currentPointIndex = 0;
        }

        StartMovement();
    }

    public Vector3 GetMovement()
    {
        return _movementDir.normalized * _speed;
    }

    public void Movement()
    {
        _movementDir = GetDir(_pathPoints[_currentPointIndex], transform);

        transform.position += _movementDir.normalized * Time.deltaTime * _speed;
    }

    
    [Serializable]struct PlatformWayPoints
    {
        Transform _positions;

        float _travelTime;
    }
}

