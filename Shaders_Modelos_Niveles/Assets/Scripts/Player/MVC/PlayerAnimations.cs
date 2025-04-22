using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations
{
    Transform _mesh;
    Animator animator;
    BasePlayer _basePlayer;
    string walk;
    string idle;
    string fall;
    string jump;

    public PlayerAnimations(BasePlayer basePlayer,Animator newAnim,Transform mesh, string newWalk, string newIdle, string newFall, string newJump) 
    {
        animator = newAnim;
        _mesh = mesh;
        _basePlayer = basePlayer;
        walk = newWalk;
        idle = newIdle;
        fall = newFall;
        jump = newJump;
    }

    public void FakeStart()
    {
        EventManager.Subscribe("OnGroundedEnter", PlayIdleAnim);
        EventManager.Subscribe("OnWalkEnter", PlayWalkAnim);
        EventManager.Subscribe("OnJumpEnter", PlayJumpAnim);
        EventManager.Subscribe("OnFallEnter", PlayFallAnim);

        _basePlayer.OnHorizontalInputChange += RotateMesh;
    }

    void RotateMesh(float horizontal)
    {
        switch (horizontal)
        {
            case 0:
                _mesh.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case > 0:
                _mesh.rotation = Quaternion.Euler(0, 120, 0);
                break;
            case < 0:
                _mesh.rotation = Quaternion.Euler(0, 240, 0);
                break;
        }
    }

    void PlayAnim(string anim)
    {
        
        animator.SetBool(walk, false);
        animator.SetBool(idle, false);
        animator.SetBool(jump, false);
        animator.SetBool(fall, false);

        animator.SetBool(anim, true);
    }

    void PlayWalkAnim(params object[] nada)
    {
        PlayAnim(walk);
    }

    void PlayIdleAnim(params object[] nada)
    {
        PlayAnim(idle);
    }

    void PlayFallAnim(params object[] nada)
    {
        PlayAnim(fall);
    }

    void PlayJumpAnim(params object[] nada)
    {
        PlayAnim(jump);
    }

    public void FakeOnDestroy()
    {
        EventManager.Unsubscribe("OnGroundedEnter", PlayIdleAnim);
        EventManager.Unsubscribe("OnWalkEnter", PlayWalkAnim);
        EventManager.Unsubscribe("OnJumpEnter", PlayJumpAnim);
        EventManager.Unsubscribe("OnFallEnter", PlayFallAnim);
    }

    
}
