using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamCollision : MonoBehaviour
{

    public float min_dist = 1.0f;
    public float max_dist = 4.0f;
    public float dist;
    public float smooth = 10.0f;

    public Vector3 dolly_dir_adjust;
    Vector3 dolly_dir;

    void Awake()
    {
        dolly_dir = transform.localPosition.normalized;
        dist = transform.localPosition.magnitude;
    }

    void Update()
    {
        Vector3 desired_cam_pos = transform.parent.TransformPoint(dolly_dir * max_dist);
        RaycastHit hit;

        if (Physics.Linecast(transform.parent.position, desired_cam_pos, out hit))
        {
            dist = Mathf.Clamp((hit.distance * 0.9f), min_dist, max_dist);
        }
        else
        {
            dist = max_dist;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, dolly_dir * dist, Time.deltaTime * smooth);
    }
}
