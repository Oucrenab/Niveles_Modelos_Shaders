using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinModel
{

    CoinBase _myCoin;
    CoinView _myCoinView;
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
        if(_myCoinView.pickedUp)return;

        _myCoin.CoinPickUp();
    }

    public CoinModel SetView(CoinView newView)
    {
        _myCoinView = newView;

        return this;
    }

    public void Save()
    {
        //Debug.Log("Player Guardado");
        _mementoState.Rec(_myCoinView.pickedUp, _renderer.enabled);

    }

    public void Load()
    {
        if (!_mementoState.IsRemember()) return;
        //Debug.Log("Player Cargado");

        var remember = _mementoState.Remember();

        _myCoinView.pickedUp = (bool)remember.parameters[0];
        _renderer.enabled = (bool)remember.parameters[1];

        Debug.Log($"{(bool)remember.parameters[1]} {_renderer.enabled}");
    }
}
