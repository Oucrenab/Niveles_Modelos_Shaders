using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void GetDamage(DamageTipe tipe);
}

public enum DamageTipe
{
    Fire,
    Cristal,
    Electric,
    None
}
