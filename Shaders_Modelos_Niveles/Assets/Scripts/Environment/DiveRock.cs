using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiveRock : MonoBehaviour , IDiveable
{
    //[SerializeField] Transform _targetDir;
    [SerializeField] float _bounceStrg;
    [SerializeField] float _bounceDuration;

    public void Dived(Transform targetObj)
    {
        //var dir = _targetDir.position - transform.position;
        //dir.z = 0;

        var finalStrg = _bounceStrg / _bounceDuration;

        targetObj.GetComponent<IBounce>().Bounce(Vector3.up, finalStrg, _bounceDuration);
    }
}
