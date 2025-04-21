using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations
{
    Animator animator;
    string walk;
    string idle;
    string fall;
    string jump;

    public PlayerAnimations(Animator newAnim, string newWalk, string newIdle, string newFall, string newJump) 
    {
        animator = newAnim;
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
