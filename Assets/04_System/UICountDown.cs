using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICountDown : MonoBehaviour
{
    Text CountText;
    Animation CloseUp;
    float CountDown = 3.0f;
    int TempCount = 0;

    public float GetCountDown() { return CountDown; }

    void Start()
    {
        CountText = GetComponent<Text>();
        CountText.text = CountDown.ToString();

        CloseUp = GetComponent<Animation>();

        SetCountState();
    }

    void Update()
    {
        if(CountDown >= 0)
        {
            CountDown -= Time.deltaTime;

            if (TempCount > (int)CountDown)
            {
                TempCount = (int)CountDown;
                CloseUp.Play();
            }
        }
        CountText.text = CountDown.ToString("N1");
    }

    public void SetCountState()
    {
        CountDown = 3.0f;
        TempCount = int.Parse(CountDown.ToString());
    }
}
