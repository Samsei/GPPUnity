using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMovement : MonoBehaviour
{
    Animator anim;

    bool anim_once = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void AnimateOpen()
    {
        if (!anim_once)
        {
            anim.SetTrigger("Opening");
            anim_once = true;
        }
    }

    public void AnimateClose()
    {
        if (anim_once)
        {
            anim.SetTrigger("Closing");
            anim_once = false;
        }
    }
}
