using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Vector2 Move()
    {
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        movement = Vector2.ClampMagnitude(movement, 1);

        return movement;
    }

    public bool Jump
    {
        get
        {
            return Input.GetButtonDown("Jump");
        }
    }

    public bool Roll
    {
        get
        {
            return Input.GetButtonDown("Roll");
        }
    }

    public bool Interact
    {
        get
        {
            return Input.GetButtonDown("Interact");
        }
    }

    public bool Swap
    {
        get
        {
            return Input.GetButtonDown("Swap");
        }
    }
}
