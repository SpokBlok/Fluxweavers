using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MaikoScript : PlayerObject
{
    // Checkers for Maiko
    public bool inAquaHex; // For Maiko Passive and signatureMove Check
    HashSet<PlayerObject> maikoSelf = new HashSet<PlayerObject>();
    GameObject shieldFromCitrine;


    // Start is called before the first frame update
    void Start()
    {
        objectName = "Maiko";

        //Getting the managers
        resourceScript = GameObject.FindObjectOfType<ResourceScript>();
        phaseHandler = GameObject.FindObjectOfType<PhaseHandler>();
        maikoSelf.Add(this);

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
        maxHealth = 200;
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

        // Checkers for what hash set to call in the PhaseHandler for targetting
        skillStatusAffectsEnemies = true;
        skillStatusAffectsAllies = false;

        signatureMoveAffectsEnemies = true;
        signatureMoveAffectsAllies = false;

        isSignatureMoveAttackPhysical = false;
        signatureMoveAttackExists = false;
        signatureMoveStatusExists = true;

        skillStatusAffectsSingle = true;
        skillStatusAffectsAOE = false;

        signatureMoveStatusAffectsSingle = false;
        signatureMoveStatusAffectsAOE = true;

        myAnimator = GetComponent<Animator>();
        splashArt = GameObject.FindGameObjectWithTag("MaikoUltImage");
        Debug.Log(splashArt);

        splashArt.SetActive(false);

        //Shield from Citrine
        shieldFromCitrine = this.transform.GetChild(0).gameObject;
        shieldFromCitrine.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        //for Citrine Shield
        makeShieldActive();
    }

    public override float basicAttack(float enemyArmor, float enemyCurrentHealth, float enemyMaximumHealth)
    {
        // Mana Portion
        if (resourceScript.playerAbilityUseCheck(basicAttackMana) == true)
        {
            resourceScript.playerAbilityUseManaUpdate(basicAttackMana);
            float outputDamage = basicAttackDamage * ((100 - enemyArmor + armorPenetration) / 100);
            return outputDamage;
            //range code here when implemented
        }
        else
        {
            Debug.Log("Not Enough Mana Buddy");
            return 0;
        }
    }

    public override float skillAttack(float enemyMagicResistance)
    {
        // Mana Portion
        if (resourceScript.playerAbilityUseCheck(skillMana) == true)
        {            
            resourceScript.playerAbilityUseManaUpdate(skillMana);
            float outputDamage = skillDamage * ((100 - enemyMagicResistance  + magicPenetration) / 100);
            return outputDamage;
            //SkillStatus effect here when done
            //Range code here when implemented
        }
        else
        {
            return 0;
            //Message here not enough mana
        }
    }

    public override void skillStatus(HashSet<PlayerObject> targets)
    {
        //No need to check mana since mana is already checked in skillAttack
        StatusEffect effect = new StatusEffect();
        //Target Calculation Goes Here
        effect.instantiateAddIntEffect("movement", -1, 1, targets);
        StatusEffectHandlerScript Handler = GameObject.FindGameObjectWithTag("StatusEffectHandler").GetComponent<StatusEffectHandlerScript>();
        Handler.addStatusEffect(effect);

    }

    public override void signatureMoveStatus(HashSet<PlayerObject> targets)
    {
        //Check if in aqua hex code here
        // Mana Portion
        if (resourceScript.playerAbilityUseCheck(signatureMoveMana) == true && isMeetingFluxAffinity())
        {
            resourceScript.playerAbilityUseManaUpdate(signatureMoveMana);

            //Buff Maiko Movement
            StatusEffect effect = new StatusEffect();
            effect.instantiateAddIntEffect("movement", 1, 1, maikoSelf);
            StatusEffectHandlerScript Handler = GameObject.FindGameObjectWithTag("StatusEffectHandler").GetComponent<StatusEffectHandlerScript>();
            Handler.addStatusEffect(effect);

            //Buff Maiko armor
            effect = new StatusEffect();
            effect.instantiateMultiFloatEffect("armor", 1.45f, 2, maikoSelf);
            Handler.addStatusEffect(effect);

            //Buff Maiko magicRes
            effect = new StatusEffect();
            effect.instantiateMultiFloatEffect("magicResistance", 1.45f, 2, maikoSelf);
            Handler.addStatusEffect(effect);

            //Enemy Debuff
            effect = new StatusEffect();
            effect.instantiateMultiFloatEffect("attackStat", 0.7f, 2, targets);
            Handler.addStatusEffect(effect);
            //Range code here when implemented
        }
        //range code here when implemented
    }

    public override bool isMeetingFluxAffinity()
    {
        TilesCreationScript tiles = GameObject.Find("Hextile Map").GetComponent<TilesCreationScript>();
        AspirantMovement aspirantIndices = GetComponent<AspirantMovement>();

        Hex currentHex = tiles.Tiles[aspirantIndices.currentYIndex, aspirantIndices.currentXIndex].GetComponent<Hex>();

        if (phaseHandler.aquaFluxes.Contains(currentHex.currentFlux))
            return true;

        return false;
    }
    //for Citrine shield
    public void makeShieldActive()
    {
        if (shield == 1)
        {
            shieldFromCitrine.SetActive(true);
        }
        else
        {
            shieldFromCitrine.SetActive(false);
        }
    }
}
