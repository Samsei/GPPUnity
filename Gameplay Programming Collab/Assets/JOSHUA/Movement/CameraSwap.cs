using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwap : MonoBehaviour
{
    public CamFollowNormal cfn;
    public CamFollowRotate cfr;
    public Transform cam;

    InputManager input;

    public bool cam_swap = false;
    private bool reset = false;

    public float smooth = 5.0f;

    private void Start()
    {
        input = GetComponent<InputManager>();
    }

    private void Update()
    {
        if (input.Swap && !cfr.rotating)
        {
            cam_swap = !cam_swap;
            cam.transform.rotation = Quaternion.Euler(0, cfr.angle, 0);
            cfn.rotY = cfr.angle;
            cfr.angle = 0.0f;
        }
        SwapCameras();
        ResetRotation();
    }

    private void SwapCameras()
    {
        if (!cam_swap)
        {
            cfn.enabled = true;
            cfr.enabled = false;
            reset = true;
        }
        else
        {
            cfn.enabled = false;
            cfr.enabled = true;
        }
    }

    private void ResetRotation()
    {
        if (cam_swap && reset)
        {
            cfr.target_angle = 0.0f;
            reset = false;
        }
    }
}
