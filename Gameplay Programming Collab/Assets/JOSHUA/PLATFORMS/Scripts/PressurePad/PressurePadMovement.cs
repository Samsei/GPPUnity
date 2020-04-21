using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePadMovement : MonoBehaviour
{
    public Material[] mat;

    Renderer rend;
    Animator anim;

    bool anim_once_one = false;
    bool anim_once_two = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        anim = GetComponent<Animator>();
    }

    public void AnimatePress()
    {
        if (!anim_once_one)
        {
            anim.SetTrigger("Pressed");
            rend.material = mat[1];
            anim_once_one = true;

            anim_once_two = false;
        }
    }

    public void AnimateReset()
    {
        if (!anim_once_two)
        {
            anim.SetTrigger("Reset");
            rend.material = mat[0];
            anim_once_two = true;

            anim_once_one = false;
        }
    }
}
