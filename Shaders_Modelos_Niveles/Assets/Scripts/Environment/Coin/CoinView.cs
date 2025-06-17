using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinView
{
    CoinBase _myCoin;

    ParticleSystem _particle;
    AudioSource _audioSource;
    Renderer _renderer;
    Transform _transform;

    float _rotationSpeed = 25;
    bool _canRotate;

    public CoinView(CoinBase newCoin, Renderer newRenderer, Transform transform, ParticleSystem particle, AudioSource audioSource)
    {
        _myCoin = newCoin;
        _myCoin.CoinPickUp += PickUp;
        _renderer = newRenderer;
        _transform = transform;
        _particle = particle;
        _audioSource = audioSource;

    }

    public void FakeUpdate()
    {
        RotationAnim();
    }



    void RotationAnim()
    {
        if (!_canRotate) return;
        _transform.Rotate(0,_transform.rotation.y + _rotationSpeed * Time.deltaTime, 0);
    }

    public CoinView SetRotation(float speed, bool canRotate = true)
    {
        _rotationSpeed = speed;
        _canRotate = canRotate;

        return this;
    }


    void PickUp()
    {
        if (_particle != null)
        {

            Debug.Log("Particulas");
            _particle.Play();
        }

        if (_audioSource != null)
        {

            Debug.Log("Sonido");
            _audioSource.Play();
        }

        Debug.Log("Apagado");
        _canRotate = false;
        _renderer.enabled = false;
    }
}
