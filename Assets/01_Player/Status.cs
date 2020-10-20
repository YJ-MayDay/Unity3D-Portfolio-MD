using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Status
{
    private int Health;
    private int Mana;
    public int AttackDamage;
    public int Defence;

    private float MoveSpeed;

    public int GetHealth() { return Health; }
    public int GetMana() { return Mana; }
    public int GetAttackDamage() { return AttackDamage; }
    public int GetDefence() { return Defence; }
    public float GetMoveSpeed() { return MoveSpeed; }

    public int SetHealth(int value) { return Health = value; }
    public int SetMana(int value) { return Mana = value; }
    public int SetAttackDamage(int value) { return AttackDamage = value; }
    public int SetDefence(int value) { return Defence = value; }
    public float SetMoveSpeed(float value) { return MoveSpeed = value; }

    public int IncreaseHealth(int value) { return Health = Health + value; }
    public int DecreaseHealth(int value)
    {
        if (value < Defence)
            return 0;
        else
            return Health = Health - (value - Defence);
    }

    public int IncreaseMana(int value) { return Mana = Mana + value; }
    public int DecreaseMana(int value) { return Mana = Mana - value; }

    public int IncreaseAttackDamage(int value) { return AttackDamage = AttackDamage + value; }
    public int IncreaseDefence(int value) { return Defence = Defence + value; }
}
