using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineBase : MonoBehaviour, IPickeable
{

    bool _alreadyTrigger;

    public void PickUp()
    {
        if (_alreadyTrigger) return;
        _alreadyTrigger = true;

        EventManager.Trigger("LevelFinished");

        StartCoroutine(Finish(2));
    }

    IEnumerator Finish(float wait)
    {
        yield return new WaitForSeconds(wait);

        Debug.Log("Finish");
    }
}
