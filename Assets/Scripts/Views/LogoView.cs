using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoView : MonoBehaviour
{
    public void Activate(bool value)
    {
        if (value)
        {
            AnimationUtil.RestartAnimator(gameObject.GetComponent<Animator>());
        }
        gameObject.SetActive(value);
    }

    void Update()
    {
        if (Input.anyKey)
        {
            Camera.main.GetComponent<Heroes>().LogoAnimationFinished();
        }
    }
}

