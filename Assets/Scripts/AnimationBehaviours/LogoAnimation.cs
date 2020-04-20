using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoAnimation : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Camera.main.GetComponent<Heroes>().LogoAnimationFinished();
    }
}
