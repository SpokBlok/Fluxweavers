using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    //Player Stats
    public int level;
    public float armor;
    public float armorPenetration;
    public float magicResistance;
    public float magicPenetration;
    public float fluxResist;
    public float attackStat;
    public int movement;
    public int control; // All players have control over 2 hexes

    // Health Related Stuff
    public HealthBar healthBar;
    public int playerHealth;
    public bool isDead;

    //Attack Stats
    public float basicAttackDamage;
    public float basicAttackMana;
    public float basicAttackRange;

    public float skillDamage;
    public float skillMana;
    public float skillRange;

    public float signatureMoveDamage;
    public float signatureMoveMana;
    public float signatureMoveRange;

    // Checkers
    public bool hasMoved; // Check if the player has moved in that turn

    //Mana & Resource Script
    public ResourceScript rs;
    public int mana;

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void manaRoundStart(int newMana)
    {
        mana = newMana;
    }
    public void IsDead()
    {
        if (playerHealth <= 0)
        {
            isDead = true;
        }
        else
        {
            isDead = false;
        }
    }
    /*public void IsAttacked(opponent.damage, opponent.attackStat, opponent.ArmorPenetration, opponent.magicPenetration)
    {
        if (opponent attack is physical)
            playerHealth = playerHealth - healthBar.TotalDamageReceived(opponent.damage, opponent.attackStat, armor, opponent.armorPenetration)
        if (opponent attack is magic)
            playerHealth = playerHealth - healthBar.TotalDamageReceived(opponent.damage, opponent.attackStat, magicResistance, opponent.magicPenetration)
        IsDead();
    }*/
}
