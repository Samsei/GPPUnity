using System.Collections;
using UnityEngine;

public class ButtonMovement : MonoBehaviour
{
    public GameObject press_effect;
    public Material[] mat;

    Renderer rend;
    Animator anim;

    bool anim_once = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        anim = GetComponent<Animator>();
    }

    public void Reset()
    {
        rend.material = mat[0];
        anim_once = false;
    }

    public void Animate()
    {
        if (!anim_once)
        {
            Instantiate(press_effect, transform.position, transform.rotation);
            anim.SetTrigger("Pressed");
            rend.material = mat[1];
            anim_once = true;
        }
    }

}
