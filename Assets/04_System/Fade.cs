using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    bool IsFadein = true;
    Image FadeIn;
    // Start is called before the first frame update
    void Start()
    {
        FadeIn = GetComponent<Image>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFadein)
        {
            if (FadeIn.color.a > 0f)
            {
                float Alpha = FadeIn.color.a;
                FadeIn.color = new Color(0.0f, 0.0f, 0.0f, Alpha - (Time.deltaTime / 2));
            }
        }
    }
}
