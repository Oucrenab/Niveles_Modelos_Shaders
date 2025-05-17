using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BounceShartModel
{
    [SerializeField] Transform _targetDir;
    //[SerializeField] float _bounceStrg;
    [SerializeField] float _bounceDuration;
    [SerializeField] float _diveDuration;

    [SerializeField] Transform _transform;
    [SerializeField] Collider _collider;
    [SerializeField] BounceShart BounceShart;
    [SerializeField] float _force;
    [SerializeField] float _refreshRate;

    public BounceShartModel(Transform transform, Transform targetDir, float dur, Collider collider, BounceShart bounceShart, float force, float dura)
    {
        _transform = transform;
        _targetDir = targetDir;
        _bounceDuration = dur;

        _collider = collider;
        BounceShart = bounceShart;
        _force = force;
        _diveDuration = dura;
    }

    public void Dashed(Transform targetObj)
    {
        var dir = _targetDir.position - _transform.position;
        dir.z = 0;

        var finalStrg = dir.magnitude / _bounceDuration;

        targetObj.GetComponent<IBounce>().Bounce(dir.normalized, finalStrg, _bounceDuration);
    }


    bool _dived = false;
    public void Dived()
    {
        if (_dived) return;
        _dived = true;

        Debug.Log("AHHHHHHHHHHHHHHH");
        _collider.isTrigger = true;

        //var force = Vector3.up * _force;

        //_rb.velocity = Vector3.zero;
        //_rb.AddForce(force, ForceMode.VelocityChange);

        BounceShart.CallCoroutine(DivedMovement(_diveDuration));
    }

    IEnumerator DivedMovement(float time)
    {
        //while(_rb.velocity.sqrMagnitude > 0f)
        //{
        //    Debug.Log("Moviendome");

        //    yield return null;
        //} 

        float duration = time;
        var dir = Vector3.up * (_force / time);
        Vector3 initialPos = _transform.position;

        while(duration > 0)
        {
            _transform.position += (duration>time/2)? dir * Time.deltaTime : dir * -Time.deltaTime ;

            //duration -= _refreshRate;
            duration -= Time.deltaTime;
            //yield return new WaitForSeconds(_refreshRate);
            yield return null;
        }

        _transform.position = initialPos;

        Debug.Log("Quieto");
        duration = 0;


        _collider.isTrigger = false;
        _dived = false;
    }

}
