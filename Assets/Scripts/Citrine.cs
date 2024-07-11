using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Citrine : PlayerObject
{
    // Checkers
    public bool hasMoved; // Check if the player has moved in that turn
    public bool inTerraHex; // For Citrine Passive and Ultimate Check

    //Mana & Resource Script
    public ResourceScript resourceScript;
    public int mana;

    // Start is called before the first frame update
    void Start()
    {
        //Player Stats
        level = 1;
        armor = 8;
        armorPenetration = 0;
        magicResistance = 12;
        magicPenetration = 0;
        attackStat = 10;
        movement = 2;
        control = 2; // All players have control over 2 hexes
        isBasicAttackPhysical = false;
        isSkillAttackPhysical = false;
        skillAttackExists = false;
        skillStatusExists = true;
        isSignatureMoveAttackPhysical = false;
        signatureMoveAttackExists = false;
        signatureMoveStatusExists = true;   

        // Health Related Stuff
        health = 250;

        //Attack Stats
        
        basicAttackMana = 4;
        basicAttackRange = 1;
        
        skillMana = 10;
        skillRange = 3;

        signatureMoveMana = 20;
        signatureMoveRange = 0;

        basicAttackDamage = attackStat * 1.1f;
        //skillBuff =  (team.armorPenetration = armorPenetration + 10) & (team.magicPenetration = magicPenetration + 10);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override float basicAttack(float armor)
    {
        // Mana Portion
        if (resourceScript.playerAbilityUseCheck(basicAttackMana) == true)
        {
            resourceScript.playerAbilityUseManaUpdate(basicAttackMana);
            //Deal Damage code here
            //range code here when implemented
            return 0;
        }
        else
        {
            return 0;
            //Message here not enough mana
        }
    }

    public override float skillStatus()
    {
        // Mana Portion
        if (resourceScript.playerAbilityUseCheck(skillMana) == true)
        {
            resourceScript.playerAbilityUseManaUpdate(skillMana);
            //Deal Damage code here
            //Range code here when implemented
            return 0;
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
        // Mana Portion
        if (resourceScript.playerAbilityUseCheck(signatureMoveMana) == true)
        {
            resourceScript.playerAbilityUseManaUpdate(signatureMoveMana);
            //team.shield = Citrine.attackStat * 10.0f;
            //team.attackStat = attackStat + attack.Stat * 0.4f;
            //Range code here when implemented
            return 0;
        }
        else
        {
            return 0;
            //Message here not enough mana
        }
        //range code here when implemented
    }
}