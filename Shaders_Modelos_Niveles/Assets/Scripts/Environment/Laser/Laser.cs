using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] ParticleSystem _electricity;
    [SerializeField] ParticleSystem _auraParticles;
    [SerializeField] GameObject _aura;
    [SerializeField] Collider _hitBox;
    [SerializeField] float _waitBTCicle;
    [SerializeField] float _dmgDuration;
    [SerializeField] float _auraDuration;

    Material _auraMat;
    bool _midCicle;

    private void Start()
    {
        _auraMat = _aura.GetComponent<MeshRenderer>().material;
    }


    private void Update()
    {
        if(!_midCicle)
            StartCoroutine(Cycle());
    }

    IEnumerator Cycle()
    {
        _midCicle = true;

        _auraParticles.Play();
        float t = 0;
        
        yield return new WaitForSeconds(_auraDuration*0.2f);

        while (t< _auraDuration)
        {
            t += Time.deltaTime;

            _auraMat.SetFloat("_Alpha",Mathf.Lerp(0, .1f, t));

            yield return null;
        }

        _auraParticles.Stop();
        _auraMat.SetFloat("_Alpha", 0);


        //yield return new WaitForSeconds(_auraDuration);

        if (_hitBox)
            _hitBox.enabled = true;
        if (_electricity)
        {
            _electricity.Play();

        }

        yield return new WaitForSeconds(_dmgDuration);
        
        if(_hitBox)
            _hitBox.enabled = false;

        yield return new WaitForSeconds(_waitBTCicle);

        _midCicle = false;
    }

}
