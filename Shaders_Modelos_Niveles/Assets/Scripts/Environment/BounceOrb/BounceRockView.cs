using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceRockView
{
    BounceRock _myRock;

    //animacion
    float _animDuration;
    Material _material;
    float _baseScale;
    float _maxScale;


    public BounceRockView(BounceRock rock, Material mat, float maxScale, float dur)
    {
        _myRock = rock;
        _myRock.OnDashed += Dash;
        _material = mat;
        _baseScale = mat.GetFloat("_NoiseEscale");
        _maxScale = maxScale;
        _animDuration = dur;
    }

    void Dash()
    {
        _myRock.StartCoroutine(DashedAnim());
    }

    IEnumerator DashedAnim()
    {
        var time = _animDuration;
        var scale = _maxScale;
        _material.SetFloat("_NoiseEscale", scale);

        while (time > 0)
        {
            time -= Time.deltaTime;

            scale = Mathf.Lerp(_maxScale, _baseScale, time);

            //Debug.Log(scale);

            _material.SetFloat("_NoiseEscale", scale);

            yield return new WaitForEndOfFrame();
        }

        scale = _baseScale;
        _material.SetFloat("_NoiseEscale", scale);

    }


}
