using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsScreen : BaseScreen
{
    Lang _lang;

    //a
    [SerializeField] BaseScreen _deathMenu;

    private void Start()
    {
        _lang = LocalizationManager.instance.currentLang;
    }

    public void OpenShaderMenu()
    {
        ScreenManager.Instance.ActivateScreen(_deathMenu);
    }

    public void ChangeLanguage()
    {
        _lang++;
        if (_lang >= Lang.Length)
            _lang = 0;

        LocalizationManager.instance.ChangeLang(_lang);
    }
}
