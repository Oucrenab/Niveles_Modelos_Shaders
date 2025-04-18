using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeModel
{
    public void FakeUpdate()
    {
        
    }

    public void FakeOnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.GetDamage();
        }
    }
}
