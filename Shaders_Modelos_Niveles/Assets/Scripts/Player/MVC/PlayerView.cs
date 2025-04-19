using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView
{
    PlayerModel _myModel;
    MeshTrail _meshTrail;

    public PlayerView(PlayerModel model, ObjectPool<GameObject> pool, SkinnedMeshRenderer[] skMeshRenderer, Material trailShader, string matAlphaName, float trailTime)
    {
        _myModel = model;

        _meshTrail = new MeshTrail(this, pool, skMeshRenderer, trailShader, matAlphaName, trailTime);
    }

    public void FakeStart()
    {
        _meshTrail.FakeStart();
    }

    public void FakeUpdate()
    {
        //Debug.Log($"<color=red>Update de View</color>");

    }

    public void StartCoroutine(IEnumerator shit)
    {
        _myModel.StartCoroutine(shit);
    }

    public void FakeOnDestroy()
    {
        _meshTrail.FakeOnDestroy();
    }
}
