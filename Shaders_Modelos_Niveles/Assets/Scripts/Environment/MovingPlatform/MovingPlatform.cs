using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour, IMovingPlatform, IMemento
{
    [SerializeField] MovingPlatformModel _myModel;

    [SerializeField] Transform[] _wayPoints;
    [SerializeField] float[] _timeBtPoints;

    #region Memento
    public Vector3 GetMovement() => _myModel.GetMovement();

    public void Load(params object[] parameters) => _myModel.Load();

    public void Save(params object[] parameters) => _myModel.Save();

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


    private void Awake()
    {
        _myModel = new MovingPlatformModel(_wayPoints, _timeBtPoints, transform, new MementoState());
    }

    private void Start()
    {
        MementoSubscribe();
    }

    private void Update()
    {
        _myModel.FakeUpdate();
    }

    private void OnDestroy()
    {
        MementoUnsubscribe();
    }
}
