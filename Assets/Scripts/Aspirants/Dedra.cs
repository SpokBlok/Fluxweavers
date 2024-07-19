using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Dedra : PlayerObject
{   // Start is called before the first frame update
    public bool wasOnFolia;
    public int checkerForExit;
    public int skillCounter;
    public int signatureMoveCounter;
    public bool isSkillActive; // checks if skill is still active (should last only 3 turns)
    public bool isSignatureMoveActive; // checks if signature move was activated
    HashSet<PlayerObject> dedraSelf = new HashSet<PlayerObject>();
    public float calculatedBasicAttackDamage;

    void Start()
    {
        dedraSelf.Add(this);

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
        skillRange = 0;
        signatureMoveMana = 12;

        //checkers
        // basic attacks
        isBasicAttackPhysical = true;

        // skill
        skillStatusExists = true;
        skillStatusAffectsAllies = true;
        skillStatusAffectsSingle = true;

        // signature move
        signatureMoveStatusExists = true;
        signatureMoveAffectsAllies = true;
        signatureMoveStatusAffectsSingle = true;

        // skill and signature move activation checkers
        isSkillActive = false;
        isSignatureMoveActive = false;

        // variables specific to dedra
        skillCounter = 3;
        signatureMoveCounter = 1;
        calculatedBasicAttackDamage = 0;
    }

    public override float basicAttack(float enemyArmor, float enemyCurrentHealth, float enemyMaximumHealth)
    {

        if (!isSkillActive && !isSignatureMoveActive) // if skill and/or ultimate are not active, revert back to base damage (no buffs)
        {   
            control = 2;
            basicAttackMana = 4;
            basicAttackRange = 3;

            basicAttackDamage = attackStat;
            calculatedBasicAttackDamage = basicAttackDamage * (100 - enemyArmor + armorPenetration) / 100;
            skillDamage = attackStat * 1.65f;

            // reset counters
            skillCounter = 3;
            signatureMoveCounter = 1;
        }

        else if (isSkillActive && skillCounter > 0 && isSignatureMoveActive && signatureMoveCounter > 0)
        {
            // instantiate signature move buffs
            control = 3; // originally 2
            basicAttackMana = 2; // originally 4
            basicAttackRange = 4; // originally 3

            // skill buffs
            if (enemyCurrentHealth < enemyMaximumHealth * 0.35f)
            {
                skillDamage = attackStat * 2f;
            }

            // else,  basic attacks deal 165% of the attackStat
            else if (enemyCurrentHealth > enemyMaximumHealth * 0.35f)
            {
                skillDamage = attackStat * 1.65f;
            }
            skillCounter --;
            signatureMoveCounter --;
            isSignatureMoveActive = false;
            calculatedBasicAttackDamage = skillDamage * (100 - enemyArmor + armorPenetration) / 100;
        }

        else if (isSkillActive && skillCounter > 0)
        {
            // if opponents health is <35%, basic attacks deal 200% of the attackStat
            if (enemyCurrentHealth < enemyMaximumHealth * 0.35f)
            {
                skillDamage = attackStat * 2f;
            }

            // else,  basic attacks deal 165% of the attackStat
            else if (enemyCurrentHealth > enemyMaximumHealth * 0.35f)
            {
                skillDamage = attackStat * 1.65f;
            }
            calculatedBasicAttackDamage = skillDamage * (100 - enemyArmor + armorPenetration) / 100;
            skillCounter --;
        }

        else if (isSignatureMoveActive && signatureMoveCounter > 0)
        {
            control = 3; // originally 2
            basicAttackMana = 2; // originally 4
            basicAttackRange = 4; // originally 3
            calculatedBasicAttackDamage = basicAttackDamage * (100 - enemyArmor + armorPenetration) / 100;
            if (enemyCurrentHealth > calculatedBasicAttackDamage)
            {
                isSignatureMoveActive = false;
                signatureMoveCounter --;
            }
            else if (enemyCurrentHealth < calculatedBasicAttackDamage)
            {
                isSignatureMoveActive = true;
                signatureMoveCounter = 1;
            }
            
        }
    
        resourceScript.playerAbilityUseManaUpdate(basicAttackMana);
        return calculatedBasicAttackDamage; 
    }

    public override void skillStatus(HashSet<PlayerObject> targets)
    {
        // logic for the skill
        if (resourceScript.playerAbilityUseCheck(skillMana))
        {
            skillCounter = 3;
            resourceScript.playerAbilityUseManaUpdate(skillMana);
            if (skillCounter > 0) // the skill lasts 3 turns
            {
                isSkillActive = true;
            }

            StatusEffect armorPenetrationEffect = new StatusEffect();
            //Target Calculation Goes Here
            armorPenetrationEffect.instantiateMultiIntEffect("armorPenetration", 1.2f, 3, dedraSelf);
            StatusEffectHandlerScript Handler = GameObject.FindGameObjectWithTag("StatusEffectHandler").GetComponent<StatusEffectHandlerScript>();
            Handler.addStatusEffect(armorPenetrationEffect);
        } 
    }

    public override void signatureMoveStatus(HashSet<PlayerObject> targets)
    {
        if (resourceScript.playerAbilityUseCheck(signatureMoveMana) == true && isMeetingFluxAffinity())
        {
            control = 3; // originally 2
            basicAttackMana = 2; // originally 4
            basicAttackRange = 4; // originally 3
            signatureMoveCounter = 1;
            isSignatureMoveActive = true;
            resourceScript.playerAbilityUseManaUpdate(signatureMoveMana);
        }

        else if (!isMeetingFluxAffinity())
            Debug.Log("nice");
        else
            Debug.Log("uhh");
    }

    public override bool isMeetingFluxAffinity()
    {
        TilesCreationScript tiles = GameObject.Find("Hextile Map").GetComponent<TilesCreationScript>();
        AspirantMovement aspirantIndices = GetComponent<AspirantMovement>();

        Hex currentHex = tiles.Tiles[aspirantIndices.currentYIndex, aspirantIndices.currentXIndex].GetComponent<Hex>();

        if (phaseHandler.foliaFluxes.Contains(currentHex.currentFlux))
            return true;

        return false;
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
        if (!isSignatureMoveActive) // if ultimate is not active, revert back (no buffs)
        {   
            control = 2;
            basicAttackMana = 4;
            basicAttackRange = 3;
        } 

        if (skillCounter < 1) // if skill is not active, revert back (no buffs)
        {   
            isSkillActive = false;
        } 
        
    }

    /* things to do for tomorrow:
            - subtract damage from enemey's health using basic attack
            - find way to check if DedraPlayer just left the Folia environment
            - double check stats, and kit specifics */
}