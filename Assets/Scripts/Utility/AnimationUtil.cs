using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class AnimationUtil
{
    public static void RestartAnimator(Animator anim)
    {
        anim.Rebind();
    }
}
