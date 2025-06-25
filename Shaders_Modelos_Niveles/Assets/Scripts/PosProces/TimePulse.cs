using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TimePulse : MonoBehaviour
{
    [SerializeField] Material _pulseMat;
    [Space]
    [SerializeField] float _pulseInTime;
    [SerializeField] float _pulseOutTime;
    [SerializeField] float _maxScale;
    [SerializeField] Transform _pulseOrigin;
    [SerializeField] Vector2 _pulseOffset;
    [SerializeField] Vector2 _finalPulseOrigin;

    /* Variables del Shader
     * _Active
     * 
     * _CircleScale
     * _CirclePosition
     * 
     * _InnerCircle
     * _EffectCircle
     */

    bool _canPulsoOn = true;

    private void Start()
    {
        EventManager.Subscribe("OnTimeStopEnter", StartPulse);
        EventManager.Subscribe("OnTimeStopEnd", EndPulse);

        _canPulsoOn = true;
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.I))
    //        StartPulse();
    //    if (Input.GetKeyDown(KeyCode.O))
    //        EndPulse();
    //}

    public void StartPulse(params object[] noSeUsa)
    {
        if (_pulseMat.GetInt("_Active") == 1) return;

        _pulseMat.SetVector("_CirclePosition", WorldToScreen());

        StartCoroutine(PulseOn());
    }

    public void EndPulse(params object[] noSeUsa)
    {
        if (_pulseMat.GetInt("_Active") != 1) return;

        _pulseMat.SetVector("_CirclePosition", WorldToScreen());

        StartCoroutine(PulseOff());
    }

    Vector2 WorldToScreen()
    {
        Camera cam = Camera.main;

        _finalPulseOrigin = new Vector2(_pulseOrigin.position.x, _pulseOrigin.position.y) + _pulseOffset;

        Vector2 pos = cam.WorldToScreenPoint(_finalPulseOrigin);


        pos.x = (pos.x - Screen.width/2) / Screen.width;
        pos.y = (pos.y - Screen.height/2) / Screen.height;

        return -pos;
    }

    IEnumerator PulseOn()
    {
        var time = 0f;

        _pulseMat.SetInt("_Active", 1);
        _pulseMat.SetFloat("_InnerCircle", 0);

        //_pulseMat.SetInt("_Invert", 0);

        while (time < _pulseInTime && _pulseMat.GetInt("_Active") == 1 && _canPulsoOn)
        {
            var scale = _maxScale * (time / _pulseInTime);

            //Debug.Log($"ON Scale {scale}");
            _pulseMat.SetFloat("_EffectCircle", scale);

            time += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator PulseOff()
    {
        _canPulsoOn = false;
        var time = 0f;

        //_pulseMat.SetInt("_Invert", 1);
        //var effectArea = _pulseMat.GetFloat("_EffectCircle");


        while (time < _pulseOutTime)
        {
            var scale = _maxScale * (time / _pulseOutTime);
            //var effectScale = effectArea - effectArea * (time / _pulseOutTime);
            //Debug.Log($"OFF Scale {scale}");

            _pulseMat.SetFloat("_InnerCircle", scale);
            //_pulseMat.SetFloat("_EffectCircle", effectScale);

            time += Time.deltaTime;
            yield return null;
        }

        _pulseMat.SetInt("_Active", 0);
        _canPulsoOn = true;
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe("OnTimeStopEnter", StartPulse);
        EventManager.Unsubscribe("OnTimeStopEnd", EndPulse);
    }
}
