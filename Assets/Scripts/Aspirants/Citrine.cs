using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Citrine : PlayerObject
{
    // Checkers
    public bool inTerraHex; // For Citrine Passive and Ultimate Check
     
    HashSet<PlayerObject> citrineSelf = new  HashSet<PlayerObject>();


    // Start is called before the first frame update
    void Start()
    {
        resourceScript = GameObject.FindObjectOfType<ResourceScript>();
        phaseHandler = GameObject.FindObjectOfType<PhaseHandler>();
        citrineSelf.Add(this);
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
        skillStatusAffectsEnemies = false;   
        skillStatusAffectsAllies = true;
        signatureMoveAffectsEnemies = false;
        signatureMoveAffectsAllies = true;
        skillStatusAffectsSingle = false;
        skillStatusAffectsAOE = true;
        signatureMoveStatusAffectsSingle = false;
        signatureMoveStatusAffectsAOE = true;  

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

    public override float basicAttack(float mr, float enemyCurrentHealth, float enemyMaximumHealth)
    {
        // Mana Portion
        if (resourceScript.playerAbilityUseCheck(basicAttackMana) == true)
        {
            resourceScript.playerAbilityUseManaUpdate(basicAttackMana);
            float outputDamage = basicAttackDamage * (100 - mr + magicPenetration) / 100; // since damage is magic damage
            return outputDamage;

            //range code here when implemented
        }
        else
        {
            return 0;
            //You Have Insufficient Mana
        }
    }

    public override void skillStatus(HashSet<PlayerObject> targets)
    {
        // Mana Portion
        if (resourceScript.playerAbilityUseCheck(skillMana) == true)
        {
            resourceScript.playerAbilityUseManaUpdate(skillMana);

            StatusEffectHandlerScript Handler = GameObject.FindGameObjectWithTag("StatusEffectHandler").GetComponent<StatusEffectHandlerScript>();

            StatusEffect effect = new StatusEffect();
            effect.instantiateAddIntEffect("magicPenetration", 10, 2, targets);
            Handler.addStatusEffect(effect);

            effect = new StatusEffect();
            effect.instantiateAddIntEffect("armorPenetration", 10, 2, targets);
            Handler.addStatusEffect(effect);

        }
        else
        {
            
            //Message here not enough mana
        }
       
    }

    public override void signatureMoveStatus(HashSet<PlayerObject> targets)
    {
        // Mana Portion
        if (resourceScript.playerAbilityUseCheck(signatureMoveMana) == true)
        {
            resourceScript.playerAbilityUseManaUpdate(signatureMoveMana);

            StatusEffectHandlerScript Handler = GameObject.FindGameObjectWithTag("StatusEffectHandler").GetComponent<StatusEffectHandlerScript>();

            StatusEffect effect = new StatusEffect();
            effect.instantiateMultiFloatEffect("attackStat", 1.4f, 2, phaseHandler.players);
            Handler.addStatusEffect(effect);
            
            foreach (PlayerObject player in phaseHandler.players) 
            {
                player.shield = 1;
            }
        }
        else
        {
            
            //Message here not enough mana
        }
        //range code here when implemented
    }
}