using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ScoreBoardMoving : MonoBehaviour
{
    GameObject[] ScoreBoard;
    Animation[] BoardAnim;
    int Count = 0;
    float NextLoading = 1.0f;
    bool LoadingEnd = false;

    UserData TempUser;
    WWW UserData;
    WWW ScoreServer;

    Image FadeIn;
    bool IsFadeIn = true;
    List<ScoreText> score;

    struct ScoreText
    {
        public string UserName;
        public int UserLevel;
        public int Kills;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        FadeIn = GameObject.Find("Fade").GetComponent<Image>();

        StartCoroutine(PushUserData());

        BoardAnim = GetComponentsInChildren<Animation>();
    }

    IEnumerator PushUserData()
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", Singleton.Instance.GetUserData().UserID);
        form.AddField("userLevel", Singleton.Instance.NowLevel.ToString());

        UserData = new WWW("http://hi91226.dothome.co.kr/PushUserData.php", form);

        yield return UserData;
        StartCoroutine(PushScoreData());
    }

    IEnumerator PushScoreData()
    {
        TempUser = Singleton.Instance.GetUserData();

        WWWForm form = new WWWForm();
        form.AddField("userName", Singleton.Instance.GetUserData().UserID);
        form.AddField("userLevel", Singleton.Instance.NowLevel);
        form.AddField("userKills", Singleton.Instance.KillCount);

        ScoreServer = new WWW("http://hi91226.dothome.co.kr/PushScoreboard.php", form);

        yield return ScoreServer;
        StartCoroutine(MakingScoreBoard());

    }

    IEnumerator MakingScoreBoard()
    {
        WWW Test = new WWW("http://hi91226.dothome.co.kr/LoadingScoredata.php");
        yield return Test;

        ScoreBoard = GameObject.FindGameObjectsWithTag("ScoreBoard");

        string[] Check = Test.text.Split(',');
        score = new List<ScoreText>(Check.Length / 3);
        for (int i = 0; i < Check.Length / 3; i++)
        {
            ScoreText Temp;
            Temp.UserName = Check[i * 3].ToString();
            Temp.UserLevel = int.Parse(Check[i * 3 + 1].ToString());
            Temp.Kills = int.Parse(Check[i * 3 + 2].ToString());
            score.Insert(i, Temp);
        }

        score.Sort(delegate (ScoreText A, ScoreText B)
        {
            if (A.Kills < B.Kills) return 1;
            else if (A.Kills > B.Kills) return -1;
            return 0;
        });

        WritePlayerData();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFadeIn)
        {
            if (FadeIn.color.a > 0f)
            {
                float Alpha = FadeIn.color.a;
                FadeIn.color = new Color(0.0f, 0.0f, 0.0f, Alpha - (Time.deltaTime / 2));
            }
            else
                IsFadeIn = false;
        }

        if (!LoadingEnd)
            BoardScroll();
    }

    void BoardScroll()
    {
        NextLoading -= Time.deltaTime;
        if (NextLoading < 0.0f)
        {
            NextLoading = 1.0f;
            BoardAnim[Count].Play();
            Count++;

            BoardScroll();
            if (Count >= BoardAnim.Length)
            {
                LoadingEnd = true;
            }
        }
    }

    void WritePlayerData()
    {
        for(int i = 0;i< ScoreBoard.Length;i++)
        {
            Text[] text = ScoreBoard[i].GetComponentsInChildren<Text>();
            text[0].text = score[i].UserName;
            text[1].text = score[i].UserLevel.ToString();
            text[2].text = score[i].Kills.ToString();
        }
    }
}
