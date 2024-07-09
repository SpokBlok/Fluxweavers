using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class Dedra : MonoBehaviour
{

    public PlayerObject Dedra;

    // Start is called before the first frame update
    void Start()
    {
    //Player Stats
        Dedra.armor = 9;
        Dedra.armorPenetration = 5;
        Dedra.magicResistance = 9;
        Dedra.magicPenetration = 0;
        Dedra.attackStat = 40;
        Dedra.movement = 2;
        Dedra.control = 2; // All players have control over 2 hexes

        // Health Related Stuff
        Dedra.health = 120;

        //Attack Stats
        Dedra.basicAttackDamage = Dedra.attackStat;
        Dedra.basicAttackMana = 4;
        Dedra.basicAttackRange = 3;

        Dedra.skillDamage; // Deals no damage
        Dedra.skillMana = 5;
        Dedra.skillRange; // N/A (Self-Target)

        Dedra.signatureMoveDamage; // Deals no damage
        Dedra.signatureMoveMana = 12;
        Dedra.signatureMoveRange; // N/A (Self-Target)

        public bool isOnFolia;
        public int checkerForExit;
        public bool skillActivation; // checks if skill was activated
        public bool isSkillStillActive = false; // checks if skill is still active (should last only 3 turns)
        public bool signatureMoveActivation = false; // checks if signature move was activated
        
    }

    public void basicAttack()
    {
        if (!isSkillStillActive && !signatureMoveActivation) // reverts the basic attack back to normal
        {
            Dedra.basicAttackDamage = Dedra.attackStat;
            Dedra.basicAttackMana = 4;
            Dedra.basicAttackRange = 3;
        }
        /* opponent.health -= Dedra.basicAttackDamage*/
    }

    public void dedraSkill()
    {
        /* if (playerClicksOnSkill && Dedra.mana >= Dedra.skillMana)
        {
            dedra.skill = true;
        }

        // logic for the skill
        if (dedra.skillActivation = true)
        {
            int counter = 0; // this counter will be changed when the turns feature has been implemented
            while (counter > 3) // the skill lasts 3 turns
            {
                // if opponents health is <35%, basic attacks deal 200% of the attackStat
                if (opponent.currentHealth < opponent.maxHealth * 0.35)
                {
                    dedra.basicAttackDamage = dedra.attackStat * 2;
                }

                // else,  basic attacks deal 165% of the attackStat
                else 
                {
                    dedra.basicAttack = dedra.attackStat * 1.65;
                }

                // set isSkillStillActive to true
                isSkillStillActive = true;
                
                // increment counter
                counter++; 
            } 
        } */
    }

    public void dedraSignatureMove()
    {
        if (wasOnFolia && Dedra.health > 0 && Dedra.mana >= Dedra.signatureMoveMana)
        {
            Dedra.control += 1;
            Dedra.basicAttackMana -= 2;
            Dedra.basicAttackRange += 1;
            signatureMoveActivation = true;
        } 
    }

    // Update is called once per frame
    void Update()
    {    
        // skill activation if the player has <=6 mana and presses the button 
        
    }

    /* things to do for tomorrow:
            - subtract damage from enemey's health using basic attack
            - find way to check if dedra just left the Folia environment
            - double check stats, and kit specifics */
}