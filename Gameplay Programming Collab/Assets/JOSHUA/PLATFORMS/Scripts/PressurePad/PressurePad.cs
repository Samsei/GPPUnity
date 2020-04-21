using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePad : MonoBehaviour
{
    PressurePadMovement ppm;
    PlatformMovementButton pmb;

    bool start_platform = false;
    bool move_platform = false;

    void Start()
    {
        ppm = GetComponentInChildren<PressurePadMovement>();
        pmb = GetComponentInChildren<PlatformMovementButton>();
    }

    void Update()
    {
        if (start_platform)
        {
            if (!move_platform)
            {
                ppm.AnimatePress();
                pmb.button_pressed = true;
                move_platform = true;
            }
            if (pmb.reset_call)
            {
                ppm.AnimateReset();
                start_platform = false;
                move_platform = false;
                pmb.reset_call = false;
            }          
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !start_platform)
        {
            start_platform = true;
        }
    }
}
