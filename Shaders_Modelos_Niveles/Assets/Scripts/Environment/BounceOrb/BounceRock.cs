using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceRock : MonoBehaviour, IDasheable
{

    [SerializeField] BounceRockModel _myModel;
    BounceRockView _myView;

    //cosas
    [SerializeField]Transform _targetDir;
    [SerializeField]float _bounceDuration;

    //anim
    [SerializeField] float _maxDeformScale;
    [SerializeField] ParticleSystem _particle;

    public event Action OnDashed = delegate { };

    private void Awake()
    {
        _myModel = new BounceRockModel(transform, _targetDir, _bounceDuration);
        _myView = new BounceRockView(this, transform.GetComponent<Renderer>().material, _maxDeformScale, _bounceDuration, _particle);
    }

    public void Dashed(Transform a)
    {
        _myModel.Dashed(a);

        OnDashed();
    }
}
