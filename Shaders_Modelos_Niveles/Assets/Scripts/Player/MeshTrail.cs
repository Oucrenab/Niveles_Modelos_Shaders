using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTrail : MonoBehaviour
{
    public float activeTime = 2f;

    [Header("Cosas del mesh")]
    public float meshRefreshRate = 0.1f;
    public Transform positionToSpawn;
    public float meshDelayDestroy = 3f;

    [Header("Shader shit")]
    public Material mat;
    public string shaderVarRef;
    public float shaderVarRate = 0.1f;
    public float shaderVarRefreshRate = 0.05f;

    bool isTrailActive = false;
    private SkinnedMeshRenderer[] _skinnedMeshRenderers;

    private void Start()
    {
        EventManager.Subscribe("OnDashEnter", CallTrail);
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
        StartCoroutine(ActivateTrail());
    }

    public IEnumerator ActivateTrail()
    {
        var timeActive = activeTime;

        while(timeActive > 0)
        {
            timeActive -= meshRefreshRate;

            if (_skinnedMeshRenderers == null)
                _skinnedMeshRenderers = GetComponents<SkinnedMeshRenderer>();

            foreach(var renderer in _skinnedMeshRenderers)
            {
                var gObj = new GameObject();
                gObj.transform.SetLocalPositionAndRotation(positionToSpawn.position, positionToSpawn.rotation);

                var mr = gObj.AddComponent<MeshRenderer>();
                var mf = gObj.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                renderer.BakeMesh(mesh);

                mf.mesh = mesh;
                mr.material = mat;

                StartCoroutine(AnimateMAterialFloat(mr.material, 0, shaderVarRate, shaderVarRefreshRate));

                Destroy(gObj, meshDelayDestroy);
            }

            yield return new WaitForSeconds(meshRefreshRate);
        }

        isTrailActive = false;
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

    private void OnDestroy()
    {
        EventManager.Unsubscribe("OnDashEnter", CallTrail);
    }
}
