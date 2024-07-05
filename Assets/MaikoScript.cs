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
    public int basicAttackMana = 4;
    public float basicAttackRange = 1;

    public float skillDamage;
    public int skillMana = 10;
    public float skillRange;

    public float signatureMoveDamage;
    public int signatureMoveMana = 16;
    public float signatureMoveRange = 2;

    // Checkers
    public bool hasMoved; // Check if the player has moved in that turn
    public bool inAquaHex; // For Maiko Passive and Ultimate Check

    //Mana & Resource Script
    public ResourceScript resourceScript;
    public int mana;

    // Start is called before the first frame update
    void Start()
    {
        basicAttackDamage = (health * 0.08f) + ((armor + magicResistance) * 0.7f); // 8% of Maiko HP + 70% of armor + magic resistance
        skillDamage = (health * 0.18f) + (attackStat); // 18% of Maiko HP + attackStat
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
        // Mana Portion
        if (resourceScript.abilityUseCheck(resourceScript.playerManaCount, basicAttackMana) == true)
        {
            resourceScript.abilityUseManaUpdate(resourceScript.playerManaCount, basicAttackMana);
            //Deal Damage code here
            //range code here when implemented
        }
        else
        {
            //Message here not enough mana
        }
    }

    public void skillAttack()
    {
        // Mana Portion
        if (resourceScript.abilityUseCheck(resourceScript.playerManaCount, skillMana) == true)
        {
            resourceScript.abilityUseManaUpdate(resourceScript.playerManaCount, skillMana);
            //Deal Damage code here
            //Range code here when implemented
        }
        else
        {
            //Message here not enough mana
        }
        //reduce target movement by 1 for 1 round here.
    }

    public void signatureMove()
    {
        // Mana Portion
        if (resourceScript.abilityUseCheck(resourceScript.playerManaCount, signatureMoveMana) == true)
        {
            resourceScript.abilityUseManaUpdate(resourceScript.playerManaCount, signatureMoveMana);
            armor += armor * 0.45f;
            magicResistance += magicResistance * 0.45f;
            //Deal Damage code here
            //Range code here when implemented
        }
        else
        {
            //Message here not enough mana
        }
        //range code here when implemented
    }
}
