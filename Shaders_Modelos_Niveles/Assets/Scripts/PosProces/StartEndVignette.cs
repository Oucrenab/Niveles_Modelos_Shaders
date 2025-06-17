using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEndVignette : MonoBehaviour
{
    [SerializeField] Material _vignetteMat;
    [Space]
    [SerializeField] float _fadeInTime;
    [SerializeField] float _fadeOutTime;

    [SerializeField, Range(0.05f, 2f)] float _power;


    //_Power
    //2 visible, 0.05 full cerrdo

    private void Start()
    {
        ActiveVignette(true);
        StartFadeOut();

        EventManager.Subscribe("LevelFinished", StartFadeIn);
        //EventManager.Subscribe("OnRespawn", StartFadeOut);
    }

    private void OnDestroy()
    {
        ActiveVignette(false);

        EventManager.Unsubscribe("LevelFinished", StartFadeIn);

        //EventManager.Unsubscribe("OnRespawn", StartFadeOut);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            StartFadeIn();
        if (Input.GetKeyDown(KeyCode.L))
            StartFadeOut();

    }

    public void StartFadeIn(params object[] noSeUsa)
    {
        //if (_vignetteMat.GetInt("_Active") == 1) return;

        //_vignetteMat.SetVector("_OriginPosition", WorldToScreen());

        StartCoroutine(FadeIn());
    }

    public void StartFadeOut(params object[] noSeUsa)
    {
        //if (_vignetteMat.GetInt("_Active") != 1) return;

        //_vignetteMat.SetVector("_OriginPosition", WorldToScreen());

        StartCoroutine(FadeOut());
    }

    public void ActiveVignette(bool active) 
    {
        if(active)
            _vignetteMat.SetFloat("_Power", 0.05f);
        else
            _vignetteMat.SetFloat("_Power", 2);

    }

    IEnumerator FadeIn()
    {
        Debug.Log("FadeIn");
        var time = 0f;

        //_vignetteMat.SetInt("_Active", 1);
        //_vignetteMat.SetFloat("_TwirlStrenght", 3);

        float fadeValue = 2;

        while (time < _fadeInTime/* && fadeValue > 1*/)
        {

            time += Time.deltaTime;
            //vignette = _maxVignette / (time / _fadeInTime);
            fadeValue = Mathf.Lerp(2, 0.05f, time / _fadeInTime);

            //Debug.Log($"ON Vignette {fadeValue}");
            _vignetteMat.SetFloat("_Power", fadeValue);

            //fadeValue = Mathf.Lerp(0, _fade, time / _fadeInTime);
            //_vignetteMat.SetFloat("_Fade", fadeValue);


            yield return null;
        }

        _vignetteMat.SetFloat("_Power", 0.05f);

        //time = 0f;
        //fadeValue = 0;

        //while (time < _fadeInTime[1] /*&& fadeValue > _fade*/)
        //{

        //    time += Time.deltaTime;
        //    //vignette = _maxVignette / (time / _fadeInTime);
        //    fadeValue = Mathf.Lerp(0, _fade, time / _fadeInTime);

        //    //Debug.Log($"ON Fade {fadeValue}");
        //    _vignetteMat.SetFloat("_Fade", fadeValue);

        //    yield return null;
        //}

        //StartFadeOut();
    }

    IEnumerator FadeOut()
    {
        Debug.Log("Fade Out");
        //_vignetteMat.SetFloat("_TwirlStrenght", -3);


        var time = 0f;
        float fadeValue = 0.05f;


        yield return new WaitForSeconds(0.1f);


        while (time <= _fadeOutTime)
        {
            //time += Time.deltaTime;

            //fadeValue2 = Mathf.Lerp(_fade, 0f, time / (_fadeOutTime * _fadeOutPorcentage));

            //_vignetteMat.SetFloat("_Fade", fadeValue2);

            //fadeValue = Mathf.Lerp(1, _maxVignette, time / _fadeOutTime);

            //_vignetteMat.SetFloat("_Power", fadeValue);

            time += Time.deltaTime;
            fadeValue = Mathf.Lerp(0.05f, 2, time / _fadeOutTime);

            _vignetteMat.SetFloat("_Power", fadeValue);

            yield return null;
        }

        _vignetteMat.SetFloat("_Power", 2);
    }
}
