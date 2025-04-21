using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView
{

    PlayerModel _myModel;

    MeshTrail _meshTrail;
    PlayerAnimations _anims;

    ObjectPool<GameObject> _pool;


    public PlayerView(PlayerModel model, ObjectPool<GameObject> pool, SkinnedMeshRenderer[] skMeshRenderer, Material trailShader, string matAlphaName, float trailTime,
        Animator animator, string idle, string walk, string jump, string fall)
    {
        _myModel = model;

        _pool = pool;

        _meshTrail = new MeshTrail(this, pool, skMeshRenderer, trailShader, matAlphaName, trailTime);
        _anims = new PlayerAnimations(animator, walk, idle, fall, jump);
    }

    public void FakeStart()
    {
        _meshTrail.FakeStart();
        _anims.FakeStart();
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
        _anims.FakeOnDestroy();
    }
}
