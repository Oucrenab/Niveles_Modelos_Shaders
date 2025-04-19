using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BttaMovement
{
    PlayerModel playerModel;

    float _xAxis, _zAxis;
    [SerializeField] float _speed = 6;
    [SerializeField] float _jumpForce = 3;
    CharacterController _cc;

    Vector3 _direccion;

    float _gravedad = -9.8f;
    float _yVelocity;
    [SerializeField] float _gravedadMult = 0.42f;

    public BttaMovement(PlayerModel model, CharacterController cc)
    {
        _cc = cc;
        playerModel = model;
    }

    public void FakeUpdate()
    {
        Debug.Log("batta");

        _xAxis = Input.GetAxisRaw("Horizontal");
        _zAxis = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

    }

    public void FakeFixedUpdate()
    {

        Debug.Log("Fixed batta");
        Gravedad();

        Movement();

    }

    void Gravedad()
    {
        if (_cc.isGrounded == true && _yVelocity < 0)
        {
            _yVelocity = -1;
        }
        else
        {
            _yVelocity += _gravedad * _gravedadMult * Time.fixedDeltaTime;
        }

        _direccion.y = _yVelocity;
    }

    void Movement()
    {
        Vector3 dir = (BasePlayer.PlayerTransform.right * _xAxis + BasePlayer.PlayerTransform.forward * _zAxis).normalized;

        _direccion.x = dir.x;
        _direccion.z = dir.z;

        Debug.Log(_direccion);
        _cc.Move(_direccion * Time.fixedDeltaTime * _speed);
    }

    void Jump()
    {
        if (_cc.isGrounded == true)
        {
            _yVelocity += _jumpForce;
        }

    }
}
