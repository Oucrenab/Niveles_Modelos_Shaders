using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] ParticleSystem _preLaser;
    [SerializeField] Collider _hitBox;
    [SerializeField] float _cicleTime;

    bool _midCicle;

    private void Start()
    {
        
    }


    private void Update()
    {
        if(!_midCicle)
            StartCoroutine(Cycle());
    }

    IEnumerator Cycle()
    {
        _midCicle = true;

        if(_preLaser)
            _preLaser.Play();

        yield return new WaitForSeconds(_cicleTime * 0.1f);

        if (_hitBox)
            _hitBox.enabled = true;

        yield return new WaitForSeconds(_cicleTime * 0.1f);
        
        if(_hitBox)
            _hitBox.enabled = false;

        yield return new WaitForSeconds(_cicleTime * 0.8f);

        _midCicle = false;
    }

}
