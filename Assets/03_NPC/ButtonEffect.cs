using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEffect : MonoBehaviour
{
    PlayerController Player;
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void AttackUP_Click()
    {
        if (Player.GetPlayerExp() - 300 >= 0)
        {
            Player.status.SetAttackDamage(Player.status.GetAttackDamage() + 5);
            Debug.Log(Player.status.GetAttackDamage());
            Player.DecreaseExp(300);
        }
    }

    public void DefenceUP_Click()
    {
        if (Player.GetPlayerExp() - 200 >= 0)
        {
            Player.status.SetDefence(Player.status.GetDefence() + 1);
            Debug.Log(Player.status.GetDefence());
            Player.DecreaseExp(200);
        }
    }

    public void HealthUP_Click()
    {
        if (Player.GetPlayerExp() - 200 >= 0)
        {
            Player.IncreaseMaxHealth(10);
            Debug.Log(Player.GetMaxHealth());
            Player.DecreaseExp(200);
        }
    }
}
