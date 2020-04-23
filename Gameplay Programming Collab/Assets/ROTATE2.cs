using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ROTATE2 : MonoBehaviour
{
    float smooth = 5.0f;
    float tiltAngle = 0.0f;
    float angleChange = 4f;

    void Update()
    {
        // Smoothly tilts a transform towards a target rotation.
        tiltAngle += angleChange;

        // Rotate the cube by converting the angles into a quaternion.
        Quaternion target = Quaternion.Euler(tiltAngle, 18, 90);

        // Dampen towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
    }
}
