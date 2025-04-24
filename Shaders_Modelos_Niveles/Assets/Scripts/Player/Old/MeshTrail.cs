using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTrail
{
    //a
    public float activeTime = 2f;

    [Header("Cosas del mesh")]
    public float meshRefreshRate = 0.025f;
    public Transform positionToSpawn;
    public float meshDelayDestroy = 0.5f;

    [Header("Shader shit")]
    public Material mat;
    public string shaderVarRef;
    public float shaderVarRate = 0.1f;
    public float shaderVarRefreshRate = 0.005f;

    bool isTrailActive = false;
    private SkinnedMeshRenderer[] _skinnedMeshRenderers;
    //arriba son cosas del codigo default
    PlayerView _myView;
    ObjectPool<GameObject> _pool;
    
    public MeshTrail(PlayerView view , ObjectPool<GameObject> pool, SkinnedMeshRenderer[] skMeshRender, Material newMat, string newShaderVarRef, float newTime)
    {
        //Debug.Log("El trail existe");

        _myView = view;
        _pool = pool;
        _skinnedMeshRenderers = skMeshRender;
        mat = newMat;
        shaderVarRef = newShaderVarRef;
        activeTime = newTime;
    }

    public void FakeStart()
    {
        EventManager.Subscribe("OnDashEnter", CallTrail);
        EventManager.Subscribe("OnPowerDashEnter", CallTrail);
        EventManager.Subscribe("OnDiveEnter", CallTrail);
    }

    //private void Update()
    //{
    //     if(Input.GetKeyDown(KeyCode.Space) && !isTrailActive)
    //    {
    //        isTrailActive = true;
    //        StartCoroutine(ActivateTrail());
    //    }
    //}

    void CallTrail(params object[] parameters)
    {
        _myView.StartCoroutine(ActivateTrail());
    }

    public IEnumerator ActivateTrail()
    {
        var timeActive = activeTime;

        while(timeActive > 0)
        {
            timeActive -= meshRefreshRate;

            //if (_skinnedMeshRenderers == null)
                //_skinnedMeshRenderers = GetComponents<SkinnedMeshRenderer>();
                //continue;

            foreach (var renderer in _skinnedMeshRenderers)
            {
                //var gObj = new GameObject();
                //gObj.transform.SetLocalPositionAndRotation(positionToSpawn.position, positionToSpawn.rotation);

                //var mr = gObj.AddComponent<MeshRenderer>();
                //var mf = gObj.AddComponent<MeshFilter>();

                //Mesh mesh = new Mesh();
                //renderer.BakeMesh(mesh);

                //mf.mesh = mesh;
                //mr.material = mat;
                //Debug.Log($"{_pool._stock.Count}");


                var gObj = _pool.Get();
                var mr = gObj.GetComponent<MeshRenderer>();

                _myView.StartCoroutine(AnimateMAterialFloat(mr.material, 0, shaderVarRate, shaderVarRefreshRate));

                _myView.StartCoroutine(CallReturn(gObj, meshDelayDestroy));
                //_pool.Return(gObj);
                //Destroy(gObj, meshDelayDestroy);
            }

            yield return new WaitForSeconds(meshRefreshRate);
        }

        isTrailActive = false;
    }

    IEnumerator CallReturn(GameObject other , float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        
        _pool.Return(other);

    }

    IEnumerator AnimateMAterialFloat(Material mat, float goal, float rate, float refreshRate)
    {
        float valueToAnimate = mat.GetFloat(shaderVarRef);

        while (valueToAnimate > goal)
        {
            valueToAnimate -= rate;
            mat.SetFloat(shaderVarRef, valueToAnimate);
            yield return new WaitForSeconds(refreshRate);
        }
    }

    public void FakeOnDestroy()
    {
        EventManager.Unsubscribe("OnDashEnter", CallTrail);
        EventManager.Unsubscribe("OnPowerDashEnter", CallTrail);
        EventManager.Unsubscribe("OnDiveEnter", CallTrail);
    }
}
