using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaikoScript : MonoBehaviour
{
    //Player Stats
    public int level = 1;
    public float armor = 16;
    public float armorPenetration = 0;
    public float magicResistance = 11;
    public float magicPenetration = 0;
    public float attackStat = 6;
    public int movement = 2;
    public int control = 2; // All players have control over 2 hexes

    // Health Related Stuff
    public HealthBar healthBar;
    public float health = 200;

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
    public ResourceScript resourceScript;
    public int mana;

    // Start is called before the first frame update
    void Start()
    {
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
            Destroy(gameObject); // Assuming there is no resurrection mechanics. Needs revision if there is.
        }
    }

    public void IsAttacked(float opponentDamage, float opponentAttackStat, float opponentArmorPenetration, float opponentMagicPenetration, bool isPhysical)
    {
        if (isPhysical)
            health = health - healthBar.TotalDamageReceived(opponentDamage, opponentAttackStat, armor, opponentArmorPenetration);
        else
            health = health - healthBar.TotalDamageReceived(opponentDamage, opponentAttackStat, magicResistance, opponentMagicPenetration);
        IsDead();
    }

    public void basicAttack()
    {
        //code here
    }

    public void skillAttack()
    {
        //code here
    }

    public void signatureMove()
    {
        //code here
    }
}
