using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceRock : MonoBehaviour, IDasheable
{
    [SerializeField] Transform _targetDir;
    [SerializeField] float _bounceStrg;
    [SerializeField] float _bounceDuration;

    public void Dashed(Transform targetObj)
    {
        var dir = _targetDir.position - transform.position;
        dir.z = 0;

        var finalStrg = dir.magnitude / _bounceDuration;

        targetObj.GetComponent<IBounce>().Bounce(dir.normalized, finalStrg, _bounceDuration);
    }
}
