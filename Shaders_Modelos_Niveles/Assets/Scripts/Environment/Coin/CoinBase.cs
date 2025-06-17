using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CoinBase : MonoBehaviour, IMemento, IPickeable
{
    public Action CoinPickUp = delegate { };

    CoinModel _coinModel;
    CoinView _coinView;

    float _rotSpeed;

    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] AudioSource _audioSource;

    private void Awake()
    {
        var renderer = GetComponent<Renderer>();

        _coinModel = new CoinModel(this, new MementoState(), renderer);
        _coinView = new CoinView(this, renderer, transform, _particleSystem, _audioSource).SetRotation(_rotSpeed ,true);
    }

    private void Update()
    {
        
        _coinView.FakeUpdate();
    }

    #region Memento
    public void Load(params object[] parameters)=> _coinModel.Load();

    public void Save(params object[] parameters)=> _coinModel.Save();

    public void MementoSubscribe()
    {
        EventManager.Subscribe("CallMementoLoad", Load);
        EventManager.Subscribe("CallMementoSave", Save);
    }

    public void MementoUnsubscribe()
    {
        EventManager.Unsubscribe("CallMementoLoad", Load);
        EventManager.Unsubscribe("CallMementoSave", Save);
    }
    #endregion

    public void PickUp()=> _coinModel.PickUp();


}
