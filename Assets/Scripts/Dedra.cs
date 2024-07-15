using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Dedra : PlayerObject
{   // Start is called before the first frame update
    public bool wasOnFolia;
    public int checkerForExit;
    public int skillCounter = 0;
    public bool isSkillStillActive = false; // checks if skill is still active (should last only 3 turns)
    public bool isSignatureMoveActive = false; // checks if signature move was activated

    public HashSet<PlayerObject> targets;

    void Start()
    {
    //Player Stats
        armor = 9;
        armorPenetration = 5;
        magicResistance = 9;
        magicPenetration = 0;
        attackStat = 40;
        movement = 2;
        control = 2; // All players have control over 2 hexes

        // Health Related Stuff
        health = 120;

        //Attack Stats
        basicAttackDamage = attackStat;
        basicAttackMana = 4;
        basicAttackRange = 3;

        skillMana = 5;
        signatureMoveMana = 12;

    }

    public override float basicAttack(float enemyArmor, float enemyCurrentHealth, float enemyMaximumHealth)
    {
        float calculatedBasicAttackDamage = 0;
        if (!isSkillStillActive && !isSignatureMoveActive) // if skill and/or ultimate are not active, revert back to base damage (no buffs)
        {
            basicAttackDamage = attackStat;
            basicAttackMana = 4;
            basicAttackRange = 3;
            calculatedBasicAttackDamage = basicAttackDamage * (100 - enemyArmor) / 100;
        }

        else if (isSkillStillActive)
        {
            // if opponents health is <35%, basic attacks deal 200% of the attackStat
            if (enemyCurrentHealth < enemyMaximumHealth * 0.35f)
            {
                skillDamage = attackStat * 2f;
            }

            // else,  basic attacks deal 165% of the attackStat
            else 
            {
                skillDamage = attackStat * 1.65f;
            }

            calculatedBasicAttackDamage = skillDamage;
            skillCounter ++;
        }

        else if (isSignatureMoveActive)
        {
            control += 1;
            basicAttackMana -= 2;
            basicAttackRange += 1;
            isSignatureMoveActive = false;
        }
        
        return calculatedBasicAttackDamage; 
    }

    public override void skillStatus()
    {
        // logic for the skill
        if (resourceScript.playerAbilityUseCheck(skillMana) == true)
        {
            resourceScript.playerAbilityUseManaUpdate(skillMana);
            if (skillCounter < 3) // the skill lasts 3 turns
            {
                isSkillStillActive = true;
            }

            StatusEffect armorPenetrationEffect = new StatusEffect();
            targets.Add(GetComponent<PlayerObject>());
            //Target Calculation Goes Here
            armorPenetrationEffect.instantiateMultiFloatEffect("armorPenetration", this.armorPenetration = this.armorPenetration * 1.2f, 3, targets);
            StatusEffectHandlerScript Handler = GameObject.FindGameObjectWithTag("StatusEffectHandler").GetComponent<StatusEffectHandlerScript>();
            Handler.addStatusEffect(armorPenetrationEffect);
        } 
        
        else
        {
            //Message here not enough mana
        }

    }

    public void PlayerSignatureMove()
    {
        if (wasOnFolia && resourceScript.playerAbilityUseCheck(signatureMoveMana) == true)
        {
            isSignatureMoveActive = true;
            resourceScript.playerAbilityUseManaUpdate(signatureMoveMana);
        } 

        else
        {
            //Message here not enough mana
        }
    }

    /*public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Dedra)
        {
            isCharacterOnTile = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Dedra && isCharacterOnTile)
        {
            isCharacterOnTile = false;
            signatureMoveActivation = true;
        }
    }
    
    link for this on chatgpt: https://chatgpt.com/share/ada9a22b-d0c4-42bc-99bf-3036e43ba00a
    
    */

    // Update is called once per frame
    void Update()
    {    
        // skill activation if the player has <=6 mana and presses the button 
        
    }

    public void OnMouseDown()
    {
        skillStatus();
    }

    /* things to do for tomorrow:
            - subtract damage from enemey's health using basic attack
            - find way to check if DedraPlayer just left the Folia environment
            - double check stats, and kit specifics */
}