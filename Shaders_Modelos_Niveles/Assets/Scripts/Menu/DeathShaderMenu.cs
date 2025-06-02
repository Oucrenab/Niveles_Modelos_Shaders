using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DeathShaderMenu : BaseScreen
{
    [SerializeField] Material _deathMaterial;

    /*
     * animated _TimeAnimated
     * Cristal _UseCell
     * Color _Color
     * CA _CA_RGBKeep
     * Ca animated _CA_OffsetSwitch
     * color original 251 100 4
     */

    [SerializeField] RawImage _animatedImage;
    [SerializeField] RawImage _cristalImage;
    [SerializeField] RawImage _colorImage;
    [SerializeField] RawImage _CAImage;
    [SerializeField] RawImage _animatedCAImage;

    [SerializeField] Color _activeColor;
    [SerializeField] Color _inactiveColor;
    [SerializeField] Color[] _posibleColor;

    int _animated;
    int _cristal;
    int _activeCA;
    int _animatedCA;
    Color _ogColor;

    public override void Activate()
    {
        base.Activate();

        GetShaderData();
        TurnOnShader();
    }

    public override void Deactivate()
    {
        base.Deactivate();

        TurnOffShader();
    }

    private void Start()
    {
        _ogColor = _deathMaterial.GetColor("_Color");
        _posibleColor[_posibleColor.Length - 1] = _ogColor;
    }

    void TurnOnShader()
    {
        _deathMaterial.SetInt("_Active", 1);

        _deathMaterial.SetFloat("_VignettePower", 1);
    }

    void TurnOffShader()
    {
        _deathMaterial.SetInt("_Active", 0);

        _deathMaterial.SetFloat("_VignettePower", 6);

    }

    void GetShaderData()
    {
        bool state;
        _animated = _deathMaterial.GetInt("_TimeAnimated");
        if(_animated == 0 ) state = false;
        else state = true;
        SwitchImage(_animatedImage, state);

        _cristal = _deathMaterial.GetInt("_UseCell");

        if (_cristal == 0) state = false;
        else state = true;
        SwitchImage(_cristalImage, state);

        _activeCA = _deathMaterial.GetInt("_CA_RGBKeep");

        if (_activeCA >= 3) state = false;
        else state = true;
        SwitchImage(_CAImage, state);

        _animatedCA = _deathMaterial.GetInt("_CA_OffsetSwitch");

        if (_animatedCA == 0) state = false;
        else state = true;
        SwitchImage(_animatedCAImage, state);

        var color = _deathMaterial.GetColor("_Color");
        _colorImage.color = color;

    }

    void SwitchImage(RawImage image, bool state)
    {
        if(state)
        {
            image.color = _activeColor;
        }
        else
            image.color = _inactiveColor;
    }

    public void BTN_Animated()
    {
        switch (_animated)
        {
            case 0:
                _animated = 1;
                SwitchImage(_animatedImage, true);

                _deathMaterial.SetInt("_TimeAnimated", 1);

                break;
            case 1:
                _animated = 0;
                SwitchImage(_animatedImage, false);

                _deathMaterial.SetInt("_TimeAnimated", 0);

                break;
        }
    }
    
    public void BTN_Cristal()
    {
        switch (_cristal)
        {
            case 0:
                _cristal = 1;
                SwitchImage(_cristalImage, true);

                _deathMaterial.SetInt("_UseCell", 1);

                break;
            case 1:
                _cristal = 0;
                SwitchImage(_cristalImage, false);

                _deathMaterial.SetInt("_UseCell", 0);

                break;
        }
    }
    
    public void BTN_Color()
    {
        ChangeColor();
    }

    int colorIndex = 0;

    void ChangeColor()
    {
        _deathMaterial.SetColor("_Color", _posibleColor[colorIndex]);
        _colorImage.color = _posibleColor[colorIndex];

        colorIndex++;
        if(colorIndex >= _posibleColor.Length) colorIndex = 0;
    }
    
    public void BTN_ChromaticAberration()
    {
        switch (_activeCA)
        {
            case 0:
                _activeCA = 4;
                SwitchImage(_CAImage, true);

                _deathMaterial.SetInt("_CA_RGBKeep", 0);

                break;
            case >=3:
                _activeCA = 0;
                SwitchImage(_CAImage, false);

                _deathMaterial.SetInt("_CA_RGBKeep", 4);

                break;
        }
    }
    
    public void BTN_ChromaticAberrationAnimated()
    {
        switch (_animatedCA)
        {
            case 0:
                _animatedCA = 1;
                SwitchImage(_animatedCAImage, true);

                _deathMaterial.SetInt("_CA_OffsetSwitch", 1);

                break;
            case 1:
                _animatedCA = 0;
                SwitchImage(_animatedCAImage, false);

                _deathMaterial.SetInt("_CA_OffsetSwitch", 0);

                break;
        }
    }



    private void OnDestroy()
    {
        _deathMaterial.SetColor("_Color",_ogColor);
    }
}
