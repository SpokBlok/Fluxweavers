using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEditor.UI;

public class Dedra : PlayerObject
{   // Start is called before the first frame update
    public bool wasOnFolia;
    public int checkerForExit;
    public bool skillActivation; // checks if skill was activated
    public bool isSkillStillActive = false; // checks if skill is still active (should last only 3 turns)
    public bool signatureMoveActivation = false; // checks if signature move was activated

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

    public override float basicAttack(float enemyArmor)
    {
        if (!isSkillStillActive && !signatureMoveActivation) // if skill and/or ultimate are not active, revert back to base damage (no buffs)
        {
            basicAttackDamage = attackStat;
            basicAttackMana = 4;
            basicAttackRange = 3;
        }
        
        return basicAttackDamage * (100 - enemyArmor) / 100; 
    }

    public void skillAttack(float enemyArmor, float enemyCurrentHealth, float enemyMaximumHealth)
    {
        // logic for the skill
        if (resourceScript.playerAbilityUseCheck(skillMana) == true)
        {
            resourceScript.playerAbilityUseManaUpdate(skillMana);
            int counter = 0; // this counter will be changed when the turns feature has been implemented
            while (counter > 3) // the skill lasts 3 turns
            {
                // if opponents health is <35%, basic attacks deal 200% of the attackStat
                if (enemyCurrentHealth < enemyMaximumHealth * 0.35f)
                {
                    StatusEffect basicAttackBuffEffect = new StatusEffect();
                    //Target Calculation Goes Here
                    basicAttackBuffEffect.instantiateEffect("basicAttackBuff", this.basicAttackDamage = this.attackStat * 2f, 3, this);
                    StatusEffectHandlerScript Handler = GameObject.FindGameObjectWithTag("StatusEffectHandler").GetComponent<StatusEffectHandlerScript>();
                    Handler.addStatusEffect(basicAttackBuffEffect);
                    // basicAttackDamage = attackStat * 2f;
                }

                // else,  basic attacks deal 165% of the attackStat
                else 
                {
                    StatusEffect basicAttackBuffEffect = new StatusEffect();
                    //Target Calculation Goes Here
                    basicAttackBuffEffect.instantiateEffect("basicAttackBuff", this.basicAttackDamage = this.attackStat * 1.65f, 3, this);
                    StatusEffectHandlerScript Handler = GameObject.FindGameObjectWithTag("StatusEffectHandler").GetComponent<StatusEffectHandlerScript>();
                    Handler.addStatusEffect(basicAttackBuffEffect);
                    // basicAttackDamage = attackStat * 1.65f;
                }

                // set isSkillStillActive to true
                isSkillStillActive = true;
                
                // increment counter
                counter++; 
            } 
        } 
        
        else
        {
            //Message here not enough mana
        }
    }

        public override void skillStatus()
    {
        
    }

    public void PlayerSignatureMove()
    {
        if (wasOnFolia && resourceScript.playerAbilityUseCheck(signatureMoveMana) == true)
        {
            StatusEffect controlBuffEffect = new StatusEffect();
            StatusEffect basicAttackManaEffect = new StatusEffect();
            StatusEffect basicAttackRangeEffect = new StatusEffect();

            controlBuffEffect.instantiateEffect("controlBuff", this.control += 1, 6969, this);
            basicAttackManaEffect.instantiateEffect("basicAttackMana", this.basicAttackMana -= 2, 6969, this);
            basicAttackRangeEffect.instantiateEffect("basicAttackRange", this.basicAttackRange += 1, 6969, this);
            StatusEffectHandlerScript Handler = GameObject.FindGameObjectWithTag("StatusEffectHandler").GetComponent<StatusEffectHandlerScript>();

            resourceScript.playerAbilityUseManaUpdate(signatureMoveMana);
            // control += 1;
            // basicAttackMana -= 2;
            // basicAttackRange += 1;
            // signatureMoveActivation = true;
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