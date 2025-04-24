using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiveRockModel : MonoBehaviour , IDiveable
{
    //[SerializeField] Transform _targetDir;
    [SerializeField] float _bounceStrg;//10
    [SerializeField] float _bounceDuration;//0.2

    public DiveRockModel(float strg, float duration)
    {
        _bounceStrg = strg;
        _bounceDuration = duration;
    }

    public void Dived(Transform targetObj)
    {
        //var dir = _targetDir.position - transform.position;
        //dir.z = 0;

        var finalStrg = _bounceStrg / _bounceDuration;

        targetObj.GetComponent<IBounce>().Bounce(Vector3.up, finalStrg, _bounceDuration);
    }
}
