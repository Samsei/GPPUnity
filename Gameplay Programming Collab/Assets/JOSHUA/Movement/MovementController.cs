using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
public class MovementController : MonoBehaviour
{
    int anim_normal_speed = 6;
    int anim_combat_speed = 4;

    float heading = 0.0f;
    float speed = 7.5f;
    float accelerate = 12.0f;
    float jump_height = 3.0f;

    float normal_speed = 7.5f;
    float roll_speed = 20.0f;
    int sprint_multiplyer = 2;

    float tS = 0.0f;
    float turn_speed = 0.0f;
    float turn_speed_min = 10f;
    float turn_speed_max = 20f;

    float roll_timer = 1.0f;
    [SerializeField] bool has_rolled = false;

    float gravity = -9.81f;

    float ground_dist = 0.4f;

    bool in_combat = false;
    public bool is_interacting = false;
    public bool in_interactable = false;
    bool has_animated = false;

    public bool on_spline = false;
    float spline_distance = 0.0f;

    int jump_count = 0;

    bool has_jumped = false;
    bool has_roll = false;

    public bool can_sprint = false;
    public bool can_double_jump = false;

    public GameObject part_sprint;
    public GameObject part_jump;

    public PathCreator path_creator;
    public EndOfPathInstruction end_of_path;

    Vector2 movement;
    Vector2 action;

    Vector3 camF;
    Vector3 camR;

    Vector3 dir;
    Vector3 intent;
    Vector3 velocity;
    Vector3 velocityXZ;

    public Transform[] cam;
    int current_cam = 0;

    CharacterController cc;
    Animator anim;
    InputManager input;
    AbilityTimer at;

    [SerializeField] float platform_amplify = 1.0f;

    void Start()
    {
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        input = GetComponent<InputManager>();
        at = GetComponent<AbilityTimer>();
    }

    void Update()
    {
        CheckInput();
        SwapCams();
        ChangeMovementMode();
        GetCamera();
        GetGravity();
        CheckMode();


        if (at.char_status == Status.double_jump)
        {
            part_jump.SetActive(true);
        }
        else
        {
            part_jump.SetActive(false);
        }

        if (at.char_status == Status.speed)
        {
            part_sprint.SetActive(true);
        }
        else
        {
            part_sprint.SetActive(false);
        }
    }

    void CheckInput()
    {
        if (input.Jump)
        {
            has_jumped = true;
        }

        if (Input.GetButtonUp("Jump"))
        {
            has_jumped = false;

        }

        if (input.Roll)
        {
            has_roll = true;
        }
        else
        {
            has_roll = false;
        }
    }

    void SwapCams()
    {
        if (on_spline)
        {
            current_cam = 1;
        }
        else
        {
            current_cam = 0;
        }
    }

    void GetCamera()
    {
        camF = cam[current_cam].forward;
        camR = cam[current_cam].right;

        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;
    }

    void GetGravity()
    {
        if (GetGround())
        {
            velocity.y = -2.0f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
        velocity.y = Mathf.Clamp(velocity.y, -50, 50);
    }

    void CanJump()
    {
        if (GetGround())
        {
            Jump();
        }
        else if (at.char_status == Status.double_jump)
        {
            if (jump_count < 2)
            {
                Jump();
            }
        }
    }

    void Jump()
    {
        if (has_jumped)
        {
            velocity.y = Mathf.Sqrt(jump_height * -2.0f * gravity);
            anim.SetTrigger("JumpTrigger");
            if (jump_count == 1)
            {
                anim.SetInteger("Jumping", 3);
            }
            else
            {
                anim.SetInteger("Jumping", 1);
            }
            jump_count++;
        }
    }

    void FixedUpdate()
    {
        if (!is_interacting)
        {
            if (!in_combat)
            {
                if (!CanRoll())
                {
                    NormalMovement();
                    CanJump();
                }
                cc.Move(velocity * Time.deltaTime);
            }
            else
            {
                CombatMovement();
            }
        }
    }

    void NormalMovement()
    {
        if (on_spline)
        {
            intent = camR * input.Move().x;
        }
        else
        {
            intent = camF * input.Move().y + camR * input.Move().x;
        }

        tS = velocity.magnitude / speed;

        turn_speed = Mathf.Lerp(turn_speed_max, turn_speed_min, tS);

        speed = normal_speed;

        if (on_spline)
        {
            if (input.Move().x != 0)
            {
                anim.SetBool("Moving", true);
                anim.SetFloat("Velocity Z", anim_normal_speed);

                Quaternion rot = Quaternion.LookRotation(intent);
                transform.rotation = Quaternion.Lerp(transform.rotation, rot, turn_speed * Time.deltaTime);

                if (at.char_status == Status.speed)
                {
                    speed *= sprint_multiplyer;
                }

            }
            else
            {
                anim.SetBool("Moving", false);
            }
        }
        else
        {
            if (input.Move().magnitude > 0)
            {
                anim.SetBool("Moving", true);
                anim.SetFloat("Velocity Z", anim_normal_speed);

                Quaternion rot = Quaternion.LookRotation(intent);
                transform.rotation = Quaternion.Lerp(transform.rotation, rot, turn_speed * Time.deltaTime);

                if (at.char_status == Status.speed)
                {
                    speed *= sprint_multiplyer;
                }

            }
            else
            {
                anim.SetBool("Moving", false);
            }
        }

        dir = transform.forward;
        velocityXZ = velocity;
        velocityXZ.y = 0;
        if (on_spline)
        {
            velocityXZ = Vector3.Lerp(velocity, dir * input.Move().x * speed, accelerate * Time.deltaTime);
        }
        else
        {
            velocityXZ = Vector3.Lerp(velocity, dir * input.Move().magnitude * speed, accelerate * Time.deltaTime);
        }

        velocity = new Vector3(velocityXZ.x, velocity.y, velocityXZ.z);

        PlatformMovement();

        if (on_spline)
        {
            SplineMovement();
        }
    }

    void PlatformMovement()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 0.4f) && hit.transform.tag == "Platform")
        {
            float momentum_x = 0.0f;
            float momentum_z = 0.0f;

            if (hit.transform.GetComponent<PlatformMovementBasic>() != null)
            {
                momentum_x = hit.transform.GetComponent<PlatformMovementBasic>().platform_dir.x * platform_amplify;
                momentum_z = hit.transform.GetComponent<PlatformMovementBasic>().platform_dir.z * platform_amplify;
            }
            else
            {
                momentum_x = hit.transform.GetComponent<PlatformMovementButton>().platform_dir.x * platform_amplify;
                momentum_z = hit.transform.GetComponent<PlatformMovementButton>().platform_dir.z * platform_amplify;
            }

            velocity.x += momentum_x;
            velocity.z += momentum_z;
            transform.parent = hit.transform;
        }
        else
        {
            transform.parent = null;
        }
    }

    void CombatMovement()
    {
        if (movement.x != 0 || movement.y != 0)
        {
            anim.SetBool("Moving", true);
            anim.SetFloat("Velocity Z", movement.y * anim_combat_speed);
            anim.SetFloat("Velocity X", movement.x * anim_combat_speed);
        }
        else
        {
            anim.SetBool("Moving", false);
        }
    }

    void SplineMovement()
    {
        if (input.Move().x != 0)
        {
            spline_distance += (input.Move().x * normal_speed) * Time.deltaTime;
            Debug.Log("Move x: " + input.Move().x);
            Debug.Log("Dist: " + spline_distance);
        }

        Vector3 new_pos = path_creator.path.GetPointAtDistance(spline_distance, end_of_path);

        transform.position = new Vector3(new_pos.x, velocity.y, new_pos.z);

        transform.rotation = path_creator.path.GetRotationAtDistance(spline_distance, end_of_path);
    }

    bool CanRoll()
    {
        if (GetGround())
        {
            if (Roll())
            {
                IsRolling();
            }
        }
        return false;
    }

    void IsRolling()
    {
        roll_timer += Time.deltaTime;

        speed = roll_speed;

        if (at.char_status == Status.speed)
        {
            speed *= (sprint_multiplyer / 1.33f);
        }

        velocityXZ = velocity;
        velocityXZ.y = 0;
        velocityXZ = Vector3.Lerp(velocity, dir * speed, accelerate * Time.deltaTime);
        velocity = new Vector3(velocityXZ.x, velocity.y, velocityXZ.z);
    }

    bool Roll()
    {
        if (has_roll && roll_timer > 0.5f && !has_rolled)
        {
            anim.SetTrigger("RollTrigger");
            anim.SetInteger("Action", 1);

            has_rolled = true;
            roll_timer = 0.0f;

            dir = transform.forward;

            cc.height = 1.25f;
            cc.center = new Vector3(0, 0.625f, 0);

            return true;
        }
        else if (roll_timer > 0.5f && has_rolled)
        {
            anim.SetInteger("Action", -1);
            has_rolled = false;

            cc.height = 2.5f;
            cc.center = new Vector3(0, 1.25f, 0);

        }

        if (roll_timer <= 0.5f && has_rolled)
        {
            return true;
        }

        return false;
    }

    void ChangeMovementMode()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            in_combat = !in_combat;
            Debug.Log("Bool now: " + in_combat);
        }
    }

    void CheckMode()
    {
        if (in_combat)
        {
            anim.SetBool("Moving", false);
            anim.applyRootMotion = true;
        }
        else
        {
            anim.SetBool("Moving", false);
            anim.applyRootMotion = false;
        }
    }

    bool GetGround()
    {
        if (cc.isGrounded)
        {
            jump_count = 0;
            anim.SetInteger("Jumping", 0);
            return true;
        }
        else
        {
            if (jump_count == 0 && !has_rolled)
            {
                anim.SetTrigger("JumpTrigger");
                anim.SetInteger("Jumping", 2);
            }
            return false;
        }
    }

    public bool Interact()
    {
        if (in_interactable && GetGround())
        {
            if (input.Interact)
            {
                return true;
            }
        }
        return false;
    }

    public void ButtonPunch(bool door_opening)
    {
        if (door_opening && !has_animated)
        {
            anim.SetInteger("Action", 3);
            anim.SetTrigger("AttackTrigger");

            has_animated = true;
        }
        else if (!door_opening && has_animated)
        {
            anim.SetInteger("Action", -1);
            has_animated = false;
            is_interacting = false;
        }
    }
}
