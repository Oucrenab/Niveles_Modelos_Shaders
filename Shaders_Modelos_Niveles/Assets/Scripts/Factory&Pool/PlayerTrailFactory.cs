using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrailFactory : Factory<GameObject>
{
    //a
    [SerializeField] ObjectPool<GameObject> _pool;

    [SerializeField] SkinnedMeshRenderer _playerSkRenderer;
    [SerializeField] Material _trailMat;
    [SerializeField] Transform _playerPos;

    public ObjectPool<GameObject> Pool {  get { return _pool; } }

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(Create, TurnOn, TurnOff, 11);
        
    }

    private void Update()
    {
        //Debug.Log($"{_pool}");
    }

    public override GameObject Create()
    {
        //Debug.Log("<color=yellow>Creado</color>");

        var gObj = new GameObject();

        var mr = gObj.AddComponent<MeshRenderer>();
        var mf = gObj.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();
        _playerSkRenderer.BakeMesh(mesh);

        mf.mesh = mesh;
        mr.material = _trailMat;

        return gObj;
    }

    public override void TurnOn(GameObject other)
    {
        //Debug.Log("<color=green>Prendido</color>");

        other.SetActive(true);

        other.transform.SetLocalPositionAndRotation(_playerPos.position, _playerPos.rotation);

        Mesh mesh = new Mesh();
        _playerSkRenderer.BakeMesh(mesh);
        other.GetComponent<MeshFilter>().mesh = mesh;

        other.GetComponent<MeshRenderer>().material.SetFloat("_Alpha",1);
    }

    public override void TurnOff(GameObject other)
    {
        //Debug.Log("<color=red>Apagado</color>");
        other.SetActive(false);
    }
}
