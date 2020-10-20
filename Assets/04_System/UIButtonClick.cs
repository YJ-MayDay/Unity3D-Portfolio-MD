using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonClick : MonoBehaviour
{
    GameObject Login;
    bool IsCreateAccount = false;
    public bool GetIsCreateAccount() { return IsCreateAccount; }
    public bool SetIsCreateAccount(bool value) { return IsCreateAccount = value; }
    void Start()
    {
        Login = GameObject.Find("LoginUI");
        Login.SetActive(false);
    }

    public void ClickStartGame()
    {
        Login.SetActive(true);
    }

    public void ClickContinueGame()
    {
        Login.SetActive(true);
    }

    public void ClickCreateAccount()
    {
        Login.SetActive(true);
        IsCreateAccount = true;
    }

    public void ClickExit()
    {
        Application.Quit();
    }
}
