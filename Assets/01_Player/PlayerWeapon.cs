using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    PlayerController Player;
    BoxCollider WeaponColl;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        WeaponColl = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(Player.GetIsAttackState() && other.tag == "Enemy")
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            enemy.PlayDamageAnim();

            enemy.DecreaseHP(Player.status.GetAttackDamage());
            //Debug.Log(enemy.status.GetHealth());
        }
    }
}
