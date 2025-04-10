using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovingPlatform
{
    public Vector3 GetMovement();
    void Movement();
}
