using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    float FadeCount = 5f;
    Image FadeImage;

    bool IsFadeOut = false;
    // Start is called before the first frame update
    void Start()
    {
        FadeImage = GameObject.Find("Fade").GetComponent<Image>();
    }

    void Update()
    {
        if(IsFadeOut)
        {
            FadeOut();
        }
    }

    public void SetChangeScene()
    {
        IsFadeOut = true;
        //SceneManager.LoadScene("Map_v0");
    }

    void FadeOut()
    {
        if (FadeImage.color.a < 1.0f)
        {
            float Alpha = FadeImage.color.a;
            FadeImage.color = new Color(0.0f, 0.0f, 0.0f, Alpha + (Time.deltaTime / 2));
        }
        else
            SceneManager.LoadScene("Map_v0");
    }
}
