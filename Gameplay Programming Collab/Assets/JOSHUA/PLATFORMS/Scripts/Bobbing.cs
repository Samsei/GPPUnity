using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobbing : MonoBehaviour
{
    [SerializeField] float sine_num = 0.0f;
    [SerializeField] float timer = 0.0f;
    [SerializeField] float amplify = 0.0125f;

    bool swap_timer = false;
    Vector3 current_pos = new Vector3(0, 0, 0);

    void Update()
    {
        UpdateSin();
        Movement();
    }

    void UpdateSin()
    {
        current_pos = transform.position;
        Timer();

        sine_num = Mathf.Sin(timer);
        current_pos.y += sine_num * amplify;
    }

    void Movement()
    {
        transform.position = current_pos;
    }

    void Timer()
    {
        if (!swap_timer)
        {
            if (timer >= 1)
            {
                swap_timer = true;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
        else
        {
            if (timer <= -1)
            {
                swap_timer = false;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
    }
}
