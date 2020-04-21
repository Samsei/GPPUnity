using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PlatformMovementBasic : MonoBehaviour
{
    public PathCreator path_creator;
    public EndOfPathInstruction end_of_path_instruction;
    public float speed = 5;
    [SerializeField] float distance_travelled;

    public Vector3 new_path = Vector3.zero;
    public Vector3 platform_dir = Vector3.zero;

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
            distance_travelled += speed * Time.deltaTime;

            new_path = path_creator.path.GetPointAtDistance(distance_travelled, end_of_path_instruction);
            platform_dir = path_creator.path.GetDirectionAtDistance(distance_travelled);

            transform.position = new Vector3(new_path.x, transform.position.y, new_path.z);
            transform.rotation = path_creator.path.GetRotationAtDistance(distance_travelled, end_of_path_instruction);
        }
    }

    void OnPathChanged()
    {
        distance_travelled = path_creator.path.GetClosestDistanceAlongPath(transform.position);
    }
}

