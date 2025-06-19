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

    float _animTime;
    Material _mat;

    public CoinView(CoinBase newCoin, Renderer newRenderer, Transform transform, ParticleSystem particle, AudioSource audioSource, float animTime, Material mat)
    {
        _myCoin = newCoin;
        _myCoin.CoinPickUp += PickUp;
        _renderer = newRenderer;
        _transform = transform;
        _particle = particle;
        _audioSource = audioSource;
        _animTime = animTime;
        _mat = mat;
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

    public bool pickedUp = false;

    void PickUp()
    {
        if(pickedUp) return;
        pickedUp = true;

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
        _myCoin.CallCoroutine(DisolveCoin(_animTime));
        //_renderer.enabled = false;
    }


    IEnumerator DisolveCoin(float duration)
    {
        float time = 0;

        while (time < duration) 
        {

            _mat.SetFloat("_CoinDisolve", Mathf.Lerp(-1, 1, time));
            time += Time.deltaTime;

            yield return null;
        }
        _mat.SetFloat("_CoinDisolve", 1);

        _renderer.enabled = false;

        _mat.SetFloat("_CoinDisolve", -1);

    }
}
