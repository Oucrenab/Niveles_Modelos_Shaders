using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiveRock : MonoBehaviour, IDiveable
{
    DiveRockModel _myModel;


    [SerializeField] float _bounceStrg;//10
    [SerializeField] float _bounceDuration;//0.2

    private void Awake()
    {
        _myModel = new DiveRockModel(_bounceStrg, _bounceDuration);
    }

    public void Dived(Transform a)
    {
        _myModel.Dived(a);
    }
}
