using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class Dedra : MonoBehaviour
{

    public PlayerObject DedraPlayer;

    // Start is called before the first frame update
    void Start()
    {
    //Player Stats
        DedraPlayer.armor = 9;
        DedraPlayer.armorPenetration = 5;
        DedraPlayer.magicResistance = 9;
        DedraPlayer.magicPenetration = 0;
        DedraPlayer.attackStat = 40;
        DedraPlayer.movement = 2;
        DedraPlayer.control = 2; // All players have control over 2 hexes

        // Health Related Stuff
        DedraPlayer.health = 120;

        //Attack Stats
        DedraPlayer.basicAttackDamage = DedraPlayer.attackStat;
        DedraPlayer.basicAttackMana = 4;
        DedraPlayer.basicAttackRange = 3;

        DedraPlayer.skillMana = 5;
        DedraPlayer.signatureMoveMana = 12;

    }

    public bool wasOnFolia;
    public int checkerForExit;
    public bool skillActivation; // checks if skill was activated
    public bool isSkillStillActive = false; // checks if skill is still active (should last only 3 turns)
    public bool signatureMoveActivation = false; // checks if signature move was activated
        

    public float basicAttack()
    {
        if (!isSkillStillActive && !signatureMoveActivation) // reverts the basic attack back to normal just in case ultimate was used
        {
            DedraPlayer.basicAttackDamage = DedraPlayer.attackStat;
            DedraPlayer.basicAttackMana = 4;
            DedraPlayer.basicAttackRange = 3;
        }
        /* opponent.health -= DedraPlayer.basicAttackDamage*/
        return DedraPlayer.basicAttackDamage;
    }

    public void DedraPlayerSkill()
    {
        /* if (playerClicksOnSkill && DedraPlayer.mana >= DedraPlayer.skillMana)
        {
            DedraPlayer.skill = true;
        }

        // logic for the skill
        if (DedraPlayer.skillActivation = true)
        {
            int counter = 0; // this counter will be changed when the turns feature has been implemented
            while (counter > 3) // the skill lasts 3 turns
            {
                // if opponents health is <35%, basic attacks deal 200% of the attackStat
                if (opponent.currentHealth < opponent.maxHealth * 0.35)
                {
                    DedraPlayer.basicAttackDamage = DedraPlayer.attackStat * 2;
                }

                // else,  basic attacks deal 165% of the attackStat
                else 
                {
                    DedraPlayer.basicAttack = DedraPlayer.attackStat * 1.65;
                }

                // set isSkillStillActive to true
                isSkillStillActive = true;
                
                // increment counter
                counter++; 
            } 
        } */
    }

    public void DedraPlayerSignatureMove()
    {
        if (wasOnFolia && DedraPlayer.health > 0 && DedraPlayer.mana >= DedraPlayer.signatureMoveMana)
        {
            DedraPlayer.control += 1;
            DedraPlayer.basicAttackMana -= 2;
            DedraPlayer.basicAttackRange += 1;
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
            - find way to check if DedraPlayer just left the Folia environment
            - double check stats, and kit specifics */
}