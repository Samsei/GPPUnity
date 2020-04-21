using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject pickup_effect;
    ButtonMovement button;
    DoorMovement door;
    Transform tp;
    GameObject player;
    MovementController mc;

    Transform cam_target;
    Transform reset_target;

    Camera main_camera;
    Camera cutscene_camera;

    GameObject can;
    FadeManager cam_fade;

    public GameObject shape_mat;
    public GameObject door_mat;
    public Material[] mat;

    bool door_open = false;
    bool door_opening = false;
    bool fade_once = false;
    public bool in_collision = false;

    float timer = 0.0f;

    [SerializeField] bool timed_door = false;
    [SerializeField] float door_timer = 0.0f;
    [SerializeField] float target_timer = 20.0f;

    void Start()
    {
        button = GetComponentInChildren<ButtonMovement>();
        door = GetComponentInChildren<DoorMovement>();

        tp = this.transform.GetChild(0).GetChild(5);
        cam_target = this.transform.GetChild(1);
        reset_target = this.transform.GetChild(4);

        player = GameObject.Find("Playable_Character");
        mc = player.GetComponent<MovementController>();

        main_camera = GameObject.Find("MainCamera").GetComponent<Camera>();
        cutscene_camera = GetComponentInChildren<Camera>();

        can = GameObject.Find("FadeManager");
        cam_fade = can.GetComponent<FadeManager>();

        if (timed_door)
        {
            shape_mat.GetComponent<MeshRenderer>().material = mat[1];
            door_mat.GetComponent<MeshRenderer>().material = mat[1];
        }
    }

    void Update()
    {
        if (!door_open)
        {
            TPPlayer();
            OpenDoor();
        }    
        else if (door_open && timed_door)
        {
            CloseDoor();
        }
    }

    void CloseDoor()
    {
        door_timer += Time.deltaTime;
        if (door_timer >= target_timer)
        {
            timer = 0.0f;
            door_timer = 0.0f;

            door_open = false;
            fade_once = false;

            door.AnimateClose();
            button.Reset();

            ResetCamera();
        }
    }

    void TPPlayer()
    {
        if (mc.Interact() && !mc.is_interacting && in_collision)
        {
            player.transform.rotation = tp.transform.rotation;
            player.transform.position = tp.transform.position;

            mc.is_interacting = true;
            door_opening = true;

            mc.ButtonPunch(true);
            SwapCameras(true);
        }
    }

    void OpenDoor()
    {
        if (door_opening)
        {
            timer += Time.deltaTime;

            if (0.4f <= timer && timer < 1.0f)
            {
                button.Animate();
            }
            else if (2.5f <= timer && timer < 3.0f)
            {
                door.AnimateOpen();
            }
            else if (5.5f <= timer && timer < 6.0f && !fade_once)
            {
                cam_fade.FadeScreen(true, 0.5f);
                fade_once = true;
            }
            else if (6.5f <= timer)
            {
                door_opening = false;
                door_open = true;
                mc.ButtonPunch(false);
                SwapCameras(false);
                cam_fade.FadeScreen(false, 0.5f);
            }

            if (timer >= 1.25f)
            {
                MoveCamera();
            }
        }
    }

    void MoveCamera()
    {
        cutscene_camera.transform.position = Vector3.Lerp(cutscene_camera.transform.position, cam_target.position, Time.deltaTime * 3.0f);
        cutscene_camera.transform.rotation = Quaternion.Lerp(cutscene_camera.transform.rotation, cam_target.rotation, Time.deltaTime * 3.0f);
    }

    void ResetCamera()
    {
        cutscene_camera.transform.position = reset_target.position;
        cutscene_camera.transform.rotation = reset_target.rotation;
    }

    void SwapCameras(bool swap)
    {
        main_camera.enabled = !swap;
        cutscene_camera.enabled = swap;
    }
}
