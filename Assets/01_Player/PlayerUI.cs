using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour
{

    PlayerController Player;

    Image HPBar;
    Image StaminaBar;
    Image PlayerExp;
    Text PlayerLevel;

    const float UIWidth = 200f;
    const float UIHeight = 25f;

    Image fade;
    float FadeOutCount = 1.0f;
    float FadeInCount = 0.0f;
    bool IsFadeOut = true;
    bool IsFadeIn = false;
    public bool SetIsFadeIn(bool value)
    {
        return IsFadeIn = value;
    }

    void Start()
    {
        Player = GetComponent<PlayerController>();

        GameObject[] obj = GameObject.FindGameObjectsWithTag("PlayerUI");
        HPBar = obj[0].GetComponent<Image>();
        StaminaBar = obj[1].GetComponent<Image>();
        PlayerExp = obj[2].GetComponent<Image>();
        PlayerLevel = GameObject.Find("PlayerLevel").GetComponent<Text>();

        fade = GameObject.Find("FadeOut").GetComponent<Image>();
    }

    public void SetUI()
    {
        SetHealthGauge();
        SetStaminaGauge();
        SetPlayerExp();

        PlayerLevel.text = Player.GetPlayerLevel().ToString();
    }

    private void SetHealthGauge()
    {
        float test = ((float)Player.status.GetHealth() / (float)Player.GetMaxHealth()) * 100;
        HPBar.rectTransform.sizeDelta = new Vector2(test * 5, UIHeight* 2);

        Text imageText = HPBar.GetComponentInChildren<Text>();
        SetGaugeText(imageText, Player.status.GetHealth());

    }

    private void SetStaminaGauge()
    {
        float test = ((float)Player.status.GetMana() / (float)Player.GetMaxMana()) * 100;
        StaminaBar.rectTransform.sizeDelta = new Vector2(test * 5, UIHeight *2);

        Text imageText = StaminaBar.GetComponentInChildren<Text>();
        SetGaugeText(imageText, Player.status.GetMana());
    }

    private void SetPlayerExp()
    {
        float test = ((float)Player.GetPlayerExp() / (float)Player.GetLevelUPExp()) * 100;
        PlayerExp.rectTransform.sizeDelta = new Vector2(test * 5, PlayerExp.rectTransform.sizeDelta.y);

        if (Player.GetPlayerExp() >= Player.GetLevelUPExp())
        {
            Instantiate(Player.GetLevelUpEffect(), transform);

            Player.SetPlayerExp(0);
            Player.SetLevelUPExp();
            Player.SetPlayerLevel();

            Player.IncreaseMaxHealth(10);
            Player.status.SetHealth(Player.GetMaxHealth());
            Player.IncreaseMaxMana(5);
            Player.status.IncreaseAttackDamage(2);
            Player.status.IncreaseDefence(1);
        }
    }

    public void SetGaugeText(Text imageText, int value)
    {
        if (value <= 0)
            imageText.enabled = false;
        else
        {
            imageText.enabled = true;
        }

        imageText.text = value.ToString();
    }
}
