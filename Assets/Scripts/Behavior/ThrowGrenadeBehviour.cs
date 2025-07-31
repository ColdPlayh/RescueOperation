using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowGrenadeBehviour : StateMachineBehaviour
{

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<PlayerControl>().aimIK.Disable();
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<PlayerControl>().aimIK.Disable();
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<PlayerControl>().aimIK.enabled = true;
    }
}
