using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Citrine : PlayerObject
{
    // Checkers
    public bool inTerraHex; // For Citrine Passive and Ultimate Check
    public float shield; // should be variable for all players
    public HashSet<PlayerObject> targets = new  HashSet<PlayerObject>();
    public Dedra dedra;

    public MaikoScript maiko;

    
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

            targets.Add(this);
            targets.Add(this.maiko);
            targets.Add(this.dedra);
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
            shield = magicResistance * 10;

            targets.Add(this);
            targets.Add(this.maiko);
            targets.Add(dedra);
            StatusEffect effect = new StatusEffect();
            effect.instantiateAddFloatEffect("shield", shield, 2, targets);

            StatusEffect effect1 = new StatusEffect();
            effect.instantiateMultiFloatEffect("attackStat", 1.4f, 2, targets);
            
        }
        else
        {
            
            //Message here not enough mana
        }
        //range code here when implemented
    }
}