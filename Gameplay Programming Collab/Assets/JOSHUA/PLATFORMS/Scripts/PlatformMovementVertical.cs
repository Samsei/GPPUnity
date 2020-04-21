using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PlatformMovementVertical : MonoBehaviour
{
    public PathCreator path_creator;
    public EndOfPathInstruction end_of_path_instruction;
    public float speed = 5;

    [SerializeField] float distance_travelled;

    public Vector3 new_path = Vector3.zero;
    public Quaternion new_rot;

    bool check_one = false;
    bool check_two = false;
    float timer = 0.0f;

    [SerializeField] float stop_dist_one = 0.0f;
    [SerializeField] float stop_dist_two = 0.0f;
    [SerializeField] float end_of_path = 0.0f;

    void Start()
    {
        if (path_creator != null)
        {
            path_creator.pathUpdated += OnPathChanged;
        }
    }

    void FixedUpdate()
    {
        if (path_creator != null)
        {
            if (Timer())
            {
                distance_travelled += speed * Time.deltaTime;

                new_path = path_creator.path.GetPointAtDistance(distance_travelled, end_of_path_instruction);
                new_rot = path_creator.path.GetRotationAtDistance(distance_travelled, end_of_path_instruction);

                transform.position = new Vector3(new_path.x, new_path.y, new_path.z);
                transform.rotation = new_rot;

                ResetChecks();
            }
            
        }
    }

    void ResetChecks()
    {
        if (distance_travelled >= end_of_path)
        {
            check_one = false;
            check_two = false;
            distance_travelled = 0;
        }
    }

    bool Timer()
    {
        if (distance_travelled >= stop_dist_one && !check_one)
        {
            timer += Time.deltaTime;
            if (timer >= 2.0f)
            {
                check_one = true;
                timer = 0.0f;
            }
            return false;
        }

        if (distance_travelled >= stop_dist_two && !check_two)
        {
            timer += Time.deltaTime;
            if (timer >= 2.0f)
            {
                check_two = true;
                timer = 0.0f;
            }
            return false;
        }

        return true;
    }

    void OnPathChanged()
    {
        distance_travelled = path_creator.path.GetClosestDistanceAlongPath(transform.position);
    }
}
