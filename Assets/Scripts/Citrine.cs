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
            float outputDamage = basicAttackDamage;
            return outputDamage;

            //range code here when implemented
        }
        else
        {
            return 0;
            //You Have Insufficient Mana
        }
    }

    public override void skillStatus()
    {
        // Mana Portion
        if (resourceScript.playerAbilityUseCheck(skillMana) == true)
        {
            resourceScript.playerAbilityUseManaUpdate(skillMana);

            PlayerObject[] targets = new PlayerObject[1];
            StatusEffect effect = new StatusEffect();
            effect.instantiateAddFloatEffect("armorPenetration", 10, 2, targets);

            StatusEffect effect1 = new StatusEffect();
            effect.instantiateAddFloatEffect("magicPenetration", 10, 2, targets);

            
        
            //Range code here when implemented (3 tile radius)
        }
        else
        {
            
            //Message here not enough mana
        }
       
    }

    public override void signatureMoveStatus()
    {
        // Mana Portion
        if (resourceScript.playerAbilityUseCheck(signatureMoveMana) == true)
        {
            resourceScript.playerAbilityUseManaUpdate(signatureMoveMana);
            //shield = Citrine.magicResistance * 10
            //attack += attackStat * 0.4f

            PlayerObject[] targets = new PlayerObject[1];
            StatusEffect effect = new StatusEffect();
            effect.instantiateMultiFloatEffect("shield", 10, 2, targets);

            StatusEffect effect1 = new StatusEffect();
            effect.instantiateMultiFloatEffect("attack", 10, 2, targets);
            
        }
        else
        {
            
            //Message here not enough mana
        }
        //range code here when implemented
    }
}