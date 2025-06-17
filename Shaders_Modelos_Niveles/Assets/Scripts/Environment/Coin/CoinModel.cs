using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CoinModel
{

    CoinBase _myCoin;
    MementoState _mementoState;
    Renderer _renderer;

    public CoinModel(CoinBase newCoin, MementoState newMemento, Renderer renderer)
    {
        _myCoin = newCoin;
        _mementoState = newMemento;
        _renderer = renderer;

    }

    public void PickUp()
    {
        if(!_renderer.enabled)return;

        _myCoin.CoinPickUp();
    }

    public void Save()
    {
        //Debug.Log("Player Guardado");
        _mementoState.Rec(_renderer.enabled);
    }

    public void Load()
    {
        if (!_mementoState.IsRemember()) return;
        //Debug.Log("Player Cargado");

        var remember = _mementoState.Remember();

        _renderer.enabled = (bool)remember.parameters[0];
    }
}
