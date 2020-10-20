using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillUI : MonoBehaviour
{
    Animator UIAnimator;
    // Start is called before the first frame update
    void Start()
    {
        UIAnimator = GameObject.Find("SkillList").GetComponent<Animator>();
    }

    public void SlideInOut(bool PlayerMove)
    {
        if(PlayerMove)
        {
            UIAnimator.Play("SlideIn");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            UIAnimator.Play("SlideOut");
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
