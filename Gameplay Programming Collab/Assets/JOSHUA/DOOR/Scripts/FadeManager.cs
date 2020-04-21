using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance { set; get; }

    Image fade_img;

    bool is_in_transition;
    bool is_showing;
    float transition;
    float duration;

    void Awake()
    {
        Instance = this;
        fade_img = GetComponentInChildren<Image>();
    }

    void Update()
    {
        if (!is_in_transition)
        {
            return;
        }
        transition += (is_showing) ? Time.deltaTime * (1 / duration) : -Time.deltaTime * (1 / duration);
        fade_img.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, transition);

        if(transition > 1 || transition < 0)
        {
            is_in_transition = false;
        }
    }

    public void FadeScreen(bool showing, float duration)
    {
        is_showing = showing;
        is_in_transition = true;
        this.duration = duration;
        transition = (is_showing) ? 0 : 1;
    }
}
