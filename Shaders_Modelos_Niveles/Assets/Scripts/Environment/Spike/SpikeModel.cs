using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeModel
{
    DamageTipe _damageTipe;

    public SpikeModel(DamageTipe tipe)
    {
        _damageTipe = tipe;
    }
    public void FakeUpdate()
    {
        
    }

    public void FakeOnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.GetDamage(_damageTipe);
        }
    }
}
