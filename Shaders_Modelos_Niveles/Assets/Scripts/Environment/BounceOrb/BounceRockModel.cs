using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BounceRockModel
{
    [SerializeField] Transform _targetDir;
    //[SerializeField] float _bounceStrg;
    [SerializeField] float _bounceDuration;

    [SerializeField] Transform _transform;

    public BounceRockModel(Transform transform, Transform targetDir, float dur)
    {
        _transform = transform;
        _targetDir = targetDir;
        _bounceDuration = dur;
    }

    public void Dashed(Transform targetObj)
    {
        var dir = _targetDir.position - _transform.position;
        dir.z = 0;

        var finalStrg = dir.magnitude / _bounceDuration;

        targetObj.GetComponent<IBounce>().Bounce(dir.normalized, finalStrg, _bounceDuration);
    }
}
