using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemName
{
    Potion_Health = 0,
    Potion_Mana = 1,
    Potion_AttackUP = 2,
    Potion_DefenceUP = 3
}

public class GetItemEffect : MonoBehaviour
{
    public ItemName Category;
    public string SelfName;
    CapsuleCollider capsule;

    void Start()
    {
        capsule = GetComponent<CapsuleCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerController Player = other.GetComponent<PlayerController>();

            switch((int)Category)
            {
                case 0:
                    {
                        if (Player.status.GetHealth() < Player.GetMaxHealth())
                        {
                            Player.status.IncreaseHealth(20);

                            if (Player.status.GetHealth() > Player.GetMaxHealth())
                                Player.status.SetHealth(Player.GetMaxHealth());

                            gameObject.SetActive(false);
                        }
                    }
                    break;

                case 1:
                    {
                        if(Player.status.GetMana() < Player.GetMaxMana())
                        {
                            Player.status.IncreaseMana(10);

                            if (Player.status.GetMana() >= Player.GetMaxMana())
                                Player.status.SetMana(Player.GetMaxMana());

                            gameObject.SetActive(false);
                        }
                    }
                    break;
                case 2:
                    {
                        Player.SetAttackDamage();
                        gameObject.SetActive(false);
                        Debug.Log(Player.status.GetAttackDamage());
                    }
                    break;

                case 3:
                    {
                        Player.SetDefenceUP();
                        gameObject.SetActive(false);
                        Debug.Log(Player.status.GetDefence());
                    }
                    break;
            }
        }
    }
}
