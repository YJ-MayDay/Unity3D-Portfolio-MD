using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILoginInterface : MonoBehaviour
{
    bool IsStartGame = false;
    bool IsContinueGame = false;
    bool IsCreateAccount = false;
    public bool SetIsStartGame(bool value)
    {
        return IsStartGame = value;
    }
    public bool SetIsContinueGame(bool value)
    {
        return IsContinueGame = value;
    }
    public bool SetIsCreateAccount(bool value)
    {
        return IsCreateAccount = value;
    }
    Image InputID;
    Image InputPassword;

    Image FadeIn;
    Image StatusPanel;
    float FadeCount = 0.0f;

    WWW Login;
    string userID, userPassword;

    bool IsLoginSuccess = false;

    void Start()
    {
        InputID = GameObject.Find("InputID").GetComponent<Image>();
        InputPassword = GameObject.Find("InputPassword").GetComponent<Image>();

        StatusPanel = GameObject.Find("StatusPanel").GetComponent<Image>();
        StatusPanel.gameObject.SetActive(false);

        FadeIn = GameObject.Find("FadeIn").GetComponent<Image>();

    }

    void Update()
    {
        if (IsLoginSuccess)
            FadeInCam();
    }

    public void CheckButton()
    {
        userID = InputID.GetComponentInChildren<InputField>().text;
        userPassword = InputPassword.GetComponentInChildren<InputField>().text;

        if (gameObject.GetComponentInParent<UIButtonClick>().GetIsCreateAccount())
            StartCoroutine(CreateAccount());
        else
        {
            StartCoroutine(LoginToDB());
        }
    }

    IEnumerator LoginToDB()
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", userID);
        form.AddField("passwordPost", userPassword);


        Login = new WWW("http://hi91226.dothome.co.kr/LoginData.php",form);

        yield return Login;

        if (Login.text == "Failed")
        {
            StatusPanel.GetComponentInChildren<Text>().text = "패스워드가 다릅니다.";
        }
        else if (Login.text == "NoAccount")
        {
            StatusPanel.GetComponentInChildren<Text>().text = "존재하지 않는 계정입니다.";
        }
        else
        {
            Singleton.Instance.SetUserData(userID, userPassword, int.Parse(Login.text));
            StatusPanel.GetComponentInChildren<Text>().text = "로그인에 성공하였습니다.";
        }
       
        StatusPanel.gameObject.SetActive(true);
    }

    IEnumerator CreateAccount()
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", userID);
        form.AddField("passwordPost", userPassword);

        Login = new WWW("http://hi91226.dothome.co.kr/CreateAccount.php", form);

        yield return Login;
        StatusPanel.GetComponentInChildren<Text>().text = Login.text;
        StatusPanel.gameObject.SetActive(true);

    }

    public void cancelButton()
    {
        InputID.GetComponentInChildren<InputField>().text = "";
        InputPassword.GetComponentInChildren<InputField>().text = "";
        this.gameObject.transform.parent.gameObject.SetActive(false);
    }

    public void StateCheck()
    {
        if (gameObject.GetComponentInParent<UIButtonClick>().GetIsCreateAccount())
        {
            gameObject.GetComponentInParent<UIButtonClick>().SetIsCreateAccount(false);

            StatusPanel.gameObject.SetActive(false);
            this.gameObject.transform.parent.gameObject.SetActive(false);

            InputID.GetComponentInChildren<InputField>().text = "";
            InputPassword.GetComponentInChildren<InputField>().text = "";
        }
        else if (Login.text == "Failed")
        {
            StatusPanel.gameObject.SetActive(false);
        }
        else if (Login.text == "NoAccount")
        {
            StatusPanel.gameObject.SetActive(false);
        }
        else
            IsLoginSuccess = true;
    }

    void FadeInCam()
    {
        if (FadeCount <= 1.0)
        {
            FadeCount += Time.deltaTime /2;
            FadeIn.color = new Color(0, 0, 0, FadeCount);
        }
        else
        {
            IsLoginSuccess = false;
            SceneManager.LoadScene("Map_v1");
        }
    }
}
