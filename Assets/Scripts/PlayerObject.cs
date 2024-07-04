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
    public float attackStat;
    public int movement;
    public int control; // All players have control over 2 hexes

    // Health Related Stuff
    public HealthBar healthBar;
    public float health;
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

    //Opponent stats
    public float opponentDamage;
    public float opponentArmorPenetration;
    public float opponentMagicPenetration;
    public float opponentAttackStat;
    public bool isPhysical;

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
        if (health <= 0)
        {
            isDead = true;
        }
        else
        {
            isDead = false;
        }
    }

    public void IsAttacked(float opponentDamage, int opponentAttackStat, int opponentArmorPenetration, int opponentMagicPenetration)
    {
        if (isPhysical)
            health = health - healthBar.TotalDamageReceived(opponentDamage, opponentAttackStat, armor, opponentArmorPenetration);
        if (!isPhysical)
            health = health - healthBar.TotalDamageReceived(opponentDamage, opponentAttackStat, magicResistance, opponentArmorPenetration);
        IsDead();
    }
}
