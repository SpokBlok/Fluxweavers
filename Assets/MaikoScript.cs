using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaikoScript : PlayerObject
{
    // Checkers for Maiko
    public bool inAquaHex; // For Maiko Passive and Ultimate Check

    // Start is called before the first frame update
    void Start()
    {
        //Player Stats
        level = 1;
        armor = 16;
        armorPenetration = 0;
        magicResistance = 11;
        magicPenetration = 0;
        attackStat = 6;
        movement = 2;
        control = 2; // All players have control over 2 hexes

        // Health Related Stuff
        health = 200;

        //Attack Stats
        basicAttackMana = 4;
        basicAttackRange = 1;

        skillDamage = 0;
        skillMana = 10;
        skillRange = 1;

        signatureMoveDamage = 1;
        signatureMoveMana = 16;
        signatureMoveRange = 2;


        basicAttackDamage = (health * 0.08f) + ((armor + magicResistance) * 0.7f); // 8% of Maiko HP + 70% of armor + magic resistance
        skillDamage = (health * 0.18f) + (attackStat); // 18% of Maiko HP + attackStat

        isBasicAttackPhysical = true;

        isSkillAttackPhysical = false;
        skillAttackExists = true;
        skillStatusExists = true;

        isSignatureMoveAttackPhysical = false;
        signatureMoveAttackExists = false;
        signatureMoveStatusExists = true;
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

    public void IsAttacked(float damageTaken)
    {
        health -= damageTaken;
        IsDead();
    }

    public override float basicAttack(int enemyArmor)
    {
        // Mana Portion
        if (resourceScript.playerAbilityUseCheck(basicAttackMana) == true)
        {
            resourceScript.playerAbilityUseManaUpdate(basicAttackMana);
            float outputDamage = basicAttackDamage * ((100 - enemyArmor) / 100);
            return outputDamage;
            //range code here when implemented
        }
        else
        {
            return 0;
            //Message here not enough mana
        }
    }

    public override float skillAttack(int enemyMagicResistance)
    {
        // Mana Portion
        if (resourceScript.playerAbilityUseCheck(skillMana) == true)
        {
            resourceScript.playerAbilityUseManaUpdate(skillMana);
            float outputDamage = basicAttackDamage * ((100 - enemyMagicResistance) / 100);
            return outputDamage;
            //SkillStatus effect here when done
            //Range code here when implemented
        }
        else
        {
            return 0;
            //Message here not enough mana
        }
        //reduce target movement by 1 for 1 round here.
    }

    public override float signatureMoveStatus()
    {
        //Check if in aqua hex code here
        // Mana Portion
        if (resourceScript.playerAbilityUseCheck(signatureMoveMana) == true)
        {
            resourceScript.playerAbilityUseManaUpdate(signatureMoveMana);
            movement += 1;// For this round only logic to be implemented in the future
            armor += armor * 0.45f;
            magicResistance += magicResistance * 0.45f;
            return 30; //Lower 30 percent of enemy attack stat
            //Deal Damage code here
            //Range code here when implemented
        }
        else
        {
            return 0;
            //Message here not enough mana
        }
        //range code here when implemented
    }
}
