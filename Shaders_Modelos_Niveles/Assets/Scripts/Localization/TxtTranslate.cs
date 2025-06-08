using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TxtTranslate : MonoBehaviour
{
    public string ID;

    TextMeshProUGUI _txt;

    private void Start()
    {
        _txt = GetComponent<TextMeshProUGUI>();

        LocalizationManager.instance.EventChangeLang += Translate;
        Translate();
    }

    void Translate()
    {
        _txt.text = LocalizationManager.instance.GetTranslate(ID);
    }
}
