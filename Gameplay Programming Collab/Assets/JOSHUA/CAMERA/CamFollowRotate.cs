using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowRotate : MonoBehaviour
{
    public GameObject cam_follow_object;
    public Transform player;

    CamCollision cc;

    public float target_angle = 0.0f;
    private const float rotation_amount = 1.5f;

    private float r_distance = 1.0f;
    private float r_speed = 1.0f;
    private float cam_move_spd = 120.0f;

    [SerializeField] private float timer = 0.0f;
    private bool reset_timer = false;

    public bool rotating = false;
    public float angle = 0.0f;

    private Vector3 mouse_delta = Vector3.zero;
    private Vector3 last_mouse_pos = Vector3.zero;

    [SerializeField] private float smooth_speed = 0.125f;
    private Vector3 offset;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cc = GetComponentInChildren<CamCollision>();
        target_angle = 0;
    }

    private void Update()
    {
        Timer();

        GetInput();

        if (timer >= 0.75f)
        {
            GetSpin();
        }

        if (target_angle != 0)
        {
            Rotate();
        }
        else
        {
            rotating = false;
            angle = transform.eulerAngles.y;
        }

        cc.max_dist = 10;
    }

    private void Timer()
    {
        timer += Time.deltaTime;

        if (reset_timer)
        {
            timer = 0.0f;
            reset_timer = false;
        }
    }

    protected void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            target_angle -= 90.0f;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            target_angle += 90.0f;
        }
    }

    protected void GetSpin()
    {
        float inputX = Input.GetAxis("RightStickHorizontal");
        float mouseX = Input.GetAxisRaw("Mouse X");

        if (mouseX <= -5 || inputX <= -1)
        {
            target_angle -= 90.0f;
            reset_timer = true;
        }
        else if (mouseX >= 5 || inputX >= 1)
        {
            target_angle += 90.0f;
            reset_timer = true;
        }
    }

    protected void Rotate()
    {
        float step = r_speed * Time.deltaTime;
        float orbit_circumfrance = 2F * r_distance * Mathf.PI;
        float distance_degrees = (r_speed / orbit_circumfrance) * 360;
        float distance_radians = (r_speed / orbit_circumfrance) * 2 * Mathf.PI;

        if (target_angle > 0)
        {
            rotating = true;
            transform.RotateAround(cam_follow_object.transform.position, Vector3.up, -rotation_amount);
            target_angle -= rotation_amount;
        }
        else if (target_angle < 0)
        {
            rotating = true;
            transform.RotateAround(cam_follow_object.transform.position, Vector3.up, rotation_amount);
            target_angle += rotation_amount;
        }
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
