using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    float smooth = 5.0f;
    float tiltAngle = 0.0f;
    float angleChange = 0.5f;

    void Update()
    {
        // Smoothly tilts a transform towards a target rotation.
        tiltAngle += angleChange;
        if (tiltAngle >= 60 || tiltAngle <= -60)
        {
            angleChange = angleChange * -1;
        }
        // Rotate the cube by converting the angles into a quaternion.
        Quaternion target = Quaternion.Euler(tiltAngle, 0, 90);

        // Dampen towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
    }
}
