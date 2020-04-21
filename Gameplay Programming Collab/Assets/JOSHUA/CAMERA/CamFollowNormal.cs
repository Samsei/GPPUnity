using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowNormal : MonoBehaviour
{
    [SerializeField] private float clamp_angle = 80.0f;
    [SerializeField] private float sens = 150.0f;

    private float mouseX;
    private float mouseY;

    private float final_inputX;
    private float final_inputZ;

    public float rotX = 0.0f;
    public float rotY = 0.0f;

    public GameObject cam_follow_object;
    CamCollision cc;

    [SerializeField] private float smooth_speed = 0.125f;
    private Vector3 offset;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 rot = transform.localRotation.eulerAngles;
        rotX = rot.x;
        rotY = rot.y;

        cc = GetComponentInChildren<CamCollision>();
    }


    private void Update()
    {
        cc.max_dist = 5;
        RotateCamera();
    }
        
    private void RotateCamera()
    {
        float inputX = Input.GetAxis("RightStickHorizontal");
        float inputZ = Input.GetAxis("RightStickVertical");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        final_inputX = inputX + mouseX;
        final_inputZ = inputZ + mouseY;

        rotX += final_inputZ * sens * Time.deltaTime;
        rotY += final_inputX * sens * Time.deltaTime;

        if (rotY >= 360.0f || rotY <= -360.0f)
        {
            rotY = 0.0f;
        }

        rotX = Mathf.Clamp(rotX, -clamp_angle, clamp_angle);

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;
    }

    private void FixedUpdate()
    {
        CameraUpdater();
    }

    private void CameraUpdater()
    {
        Transform target = cam_follow_object.transform;
        Vector3 desired_position = target.position + offset;

        Vector3 smoothed_position = Vector3.Lerp(transform.position, desired_position, smooth_speed);
        transform.position = smoothed_position;
    }
}

