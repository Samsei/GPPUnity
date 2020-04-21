using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Status
{
    no_powerup,
    speed,
    double_jump
}

public class AbilityTimer : MonoBehaviour
{
    [SerializeField] private float timer = 0.0f;
    public Status char_status;

    private void Update()
    {
        countDown();
    }

    public void updateVariables(float add_timer, int status)
    {
        timer += add_timer;
        if (timer >= 15)
        {
            timer = 15;
        }

        switch (status)
        {
            case 1:
                char_status = Status.speed;
                break;
            case 2:
                char_status = Status.double_jump;
                break;
            default:
                char_status = Status.no_powerup;
                break;
        }
    }

    private void countDown()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = 0;
            char_status = Status.no_powerup;
        }
    }
}
