using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathVignette : MonoBehaviour
{
    [SerializeField] Material _vignetteMat;
    [Space]
    [SerializeField] float[] _fadeInTime;
    [SerializeField] float _fadeOutTime;
    [SerializeField, Range (0.1f, 1f)] float _fadeOutPorcentage;
    [SerializeField,Range(-1f,0f)] float _fade;
    [SerializeField,Range(1f,6f)] float _maxVignette;
    [SerializeField] Transform _pulseOrigin;
    [SerializeField] Vector2 _pulseOffset;
    [SerializeField] Vector2 _finalPulseOrigin;
    [SerializeField] Camera cam;
    [Space]
    [SerializeField] Color[] _colors;
    /*
     * _Active
     * _Fade
     * _VignettePower
     * _OriginPosition
     * _TwirlStrenght
     * 
     */

    private void Start()
    {
        EventManager.Subscribe("OnDieEnter", StartFadeIn); 
        EventManager.Subscribe("DamageEnter", ChangeWithDMGTipe); 
        //EventManager.Subscribe("OnRespawn", StartFadeOut);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe("OnDieEnter", StartFadeIn);
        EventManager.Unsubscribe("DamageEnter", ChangeWithDMGTipe);

        _vignetteMat.SetColor("_Color", _colors[_colors.Length-1]);
        //EventManager.Unsubscribe("OnRespawn", StartFadeOut);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            StartFadeIn();
    }

    public void StartFadeIn(params object[] noSeUsa)
    {
        if (_vignetteMat.GetInt("_Active") == 1) return;

        _vignetteMat.SetVector("_OriginPosition", WorldToScreen());

        StartCoroutine(FadeIn());
    }

    public void StartFadeOut(params object[] noSeUsa)
    {
        if (_vignetteMat.GetInt("_Active") != 1) return;

        _vignetteMat.SetVector("_OriginPosition", WorldToScreen());

        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        Debug.Log("FadeIn");
        var time = 0f;

        _vignetteMat.SetInt("_Active", 1);
        _vignetteMat.SetFloat("_TwirlStrenght", 3);

        float fadeValue = _maxVignette;

        while (time < _fadeInTime[0]/* && fadeValue > 1*/)
        {

            time += Time.deltaTime;
            //vignette = _maxVignette / (time / _fadeInTime);
            fadeValue = Mathf.Lerp(_maxVignette, 1, time / _fadeInTime[0]);

            //Debug.Log($"ON Vignette {fadeValue}");
            _vignetteMat.SetFloat("_VignettePower", fadeValue);

            yield return null;
        }

        time = 0f;
        fadeValue = 0;

        while (time < _fadeInTime[1] /*&& fadeValue > _fade*/)
        {

            time += Time.deltaTime;
            //vignette = _maxVignette / (time / _fadeInTime);
            fadeValue = Mathf.Lerp(0, _fade, time / _fadeInTime[1]);

            //Debug.Log($"ON Fade {fadeValue}");
            _vignetteMat.SetFloat("_Fade", fadeValue);

            yield return null;
        }

        StartFadeOut();
    }

    IEnumerator FadeOut()
    {
        Debug.Log("Fade Out");
        _vignetteMat.SetFloat("_TwirlStrenght", -3);


        var time = 0f;
        float fadeValue = _fade;
        float fadeValue2 = _fade;
        float partialTime = _fadeOutTime * 0.5f;


        yield return new WaitForSeconds(0.1f);

        //while (time <= _fadeInTime[0] /*&& fadeValue < 0*/)
        //{

        //    //vignette = _maxVignette / (time / _fadeInTime);
        //    fadeValue = Mathf.Lerp(_fade, 0f, time / _fadeOutTime[0]);

        //    //Debug.Log($"OFF Fade {fadeValue}");
        //    _vignetteMat.SetFloat("_Fade", fadeValue);

        //    time += Time.deltaTime;
        //    yield return null;
        //}

        ////_vignetteMat.SetFloat("_Fade", 0);
        //time = 0f;
        //fadeValue = 1;

        //while (time < _fadeOutTime[1] /*&& fadeValue < _maxVignette*/)
        //{

        //    //vignette = _maxVignette / (time / _fadeInTime);
        //    fadeValue = Mathf.Lerp(1, _maxVignette, time / _fadeOutTime[1]);

        //    //Debug.Log($"OFF Vignette {fadeValue}");
        //    _vignetteMat.SetFloat("_VignettePower", fadeValue);

        //    time += Time.deltaTime;
        //    yield return null;
        //}
        //a

        while (time <= _fadeOutTime)
        {
            time += Time.deltaTime;
            //if(time <= partialTime)
            //{
            //    //fade
            //    fadeValue = Mathf.Lerp(_fade, 0f, time / partialTime);

            //    _vignetteMat.SetFloat("_Fade", fadeValue);
            //}
            //else
            //{
            //    _vignetteMat.SetFloat("_Fade", 0);
            //    //vignette
            //    fadeValue = Mathf.Lerp(1, _maxVignette, time / _fadeOutTime);

            //    _vignetteMat.SetFloat("_VignettePower", fadeValue);
            //}

            fadeValue2 = Mathf.Lerp(_fade, 0f, time / (_fadeOutTime * _fadeOutPorcentage));

            _vignetteMat.SetFloat("_Fade", fadeValue2);

            fadeValue = Mathf.Lerp(1, _maxVignette, time / _fadeOutTime);

            _vignetteMat.SetFloat("_VignettePower", fadeValue);

            yield return null;
        }

        _vignetteMat.SetInt("_Active", 0);
        _vignetteMat.SetFloat("_VignettePower", 6);



    }

    /*
     * animated _TimeAnimated
     * Cristal _UseCell
     * Color _Color
     * CA _CA_RGBKeep
     * Ca animated _CA_OffsetSwitch
     * color original 251 100 4
     */

    void ChangeWithDMGTipe(params object[] noSeUsa)
    {
        DamageTipe tipe = (DamageTipe)noSeUsa[0];

        switch (tipe)
        {
            case DamageTipe.Fire:
                _vignetteMat.SetInt("_UseCell", 0);
                _vignetteMat.SetInt("_TimeAnimated", 1);
                _vignetteMat.SetColor("_Color", _colors[(int)tipe]);

                break;
            case DamageTipe.Cristal:
                _vignetteMat.SetInt("_UseCell", 1);
                _vignetteMat.SetInt("_TimeAnimated", 0);
                _vignetteMat.SetColor("_Color", _colors[(int)tipe]);
                
                break;
            case DamageTipe.Electric:
                _vignetteMat.SetInt("_UseCell", 1);
                _vignetteMat.SetInt("_TimeAnimated", 0);
                _vignetteMat.SetColor("_Color", _colors[(int)tipe]);
                break;
            default:
                _vignetteMat.SetInt("_UseCell", 0);
                _vignetteMat.SetInt("_TimeAnimated", 1);
                //_vignetteMat.SetColor("_Color", _colors[_colors.Length-1]);
                _vignetteMat.SetColor("_Color", _colors[3]);

                break;
        }
    }

    Vector2 WorldToScreen()
    {
        _finalPulseOrigin = new Vector2(_pulseOrigin.position.x, _pulseOrigin.position.y) + _pulseOffset;

        Vector2 pos = cam.WorldToScreenPoint(_finalPulseOrigin);


        //pos.x = ((pos.x - Screen.width / 2) / Screen.width) - 0.5f;
        //pos.y = (pos.y - Screen.height / 2) / Screen.height;
        
        pos.x = pos.x / Screen.width;
        pos.y = pos.y / Screen.height;

        return -pos;
    }
}
