using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstalactitaDamage : MonoBehaviour
{
    [SerializeField] Collider _hitBox;
    [SerializeField] Collider _staticaCollider;
    [SerializeField] DamageTipe _damageTipe;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
            damageable.GetDamage(_damageTipe);
    }

    public void SetActive(bool state)
    {
        if (state)
        {
            _hitBox.enabled = true;
            _staticaCollider.enabled = false;
            return;
        }
        //a
        _hitBox.enabled = false;
        _staticaCollider.enabled = true;
    }
}
