using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsScreen : BaseScreen
{
    //a
    [SerializeField] BaseScreen _deathMenu;
    public void OpenShaderMenu()
    {
        ScreenManager.Instance.ActivateScreen(_deathMenu);
    }
}
