using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    //Player Stats
    public int level;
    public float armor;
    public int armorPenetration;
    public float magicResistance;
    public int magicPenetration;
    public float attackStat;
    public int movement;
    public int control; // All players have control over 2 hexes

    public float maxHealth; // Needed for Dedra
    public float health;

    //Attack Stats
    public float basicAttackDamage;
    public int basicAttackMana;
    public float basicAttackRange=3;

    public float skillDamage;
    public int skillMana;
    public float skillRange;

    public float signatureMoveDamage;
    public int signatureMoveMana;
    public float signatureMoveRange;

    // Checkers
    public bool hasMoved; // Check if the player has moved in that turn
    public bool isSelected; //Check if the player is selected
    public bool isBasicAttackPhysical = false;

    public bool isSkillAttackPhysical = false;
    public bool skillAttackExists = false;
    public bool skillStatusExists = false;

    public bool isSignatureMoveAttackPhysical = false;
    public bool signatureMoveAttackExists = false;
    public bool signatureMoveStatusExists = false;

    // Checkers for what hash set to call in the PhaseHandler for targetting
    public bool skillStatusAffectsEnemies = false;
    public bool skillStatusAffectsAllies = false;

    public bool signatureMoveAffectsEnemies = false;
    public bool signatureMoveAffectsAllies = false;

    // Checkers if skill status is single target or AOE
    public bool skillStatusAffectsSingle = false;
    public bool skillStatusAffectsAOE = false;

    public bool signatureMoveStatusAffectsSingle = false;
    public bool signatureMoveStatusAffectsAOE = false;

    //Mana & Resource Script
    public ResourceScript resourceScript;
    public int mana;

    // Phase Handler Script
    public PhaseHandler phaseHandler;

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        maxHealth = health;
        attackStat = 20;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void manaRoundStart(int newMana)
    {
        mana = newMana;
    }
    public void IsDead()
    {
        if (health <= 0)
        {
            Destroy(gameObject); // Assuming there is no resurrection mechanics. Needs revision if there is.
            //Death Animation plays
        }
    }

    public void IsAttacked(float opponentDamage)
    {
        health -= opponentDamage;
        IsDead();
    }

    public virtual float basicAttack(float armor, float enemyCurrentHealth, float enemyMaximumHealth)
    {
        return 0;
    }

    public virtual float skillAttack(float enemyResistStat)
    {
        return 0;
    }

    public virtual void skillStatus(HashSet<PlayerObject> targets)
    {
        
    }

    public virtual float signatureMoveAttack(float enemyResistStat)
    {
        return 0;
    }

    public virtual void signatureMoveStatus(HashSet<PlayerObject> targets)
    {
        
    }
    public void OnMouseDown()
    {
        if(this.gameObject.CompareTag("Player"))
        {   
            if(phaseHandler.playerAspirant.selectedAttack == "SkillAttack")
            {
                phaseHandler.playerAspirant.SkillAttackDamage(phaseHandler);
                phaseHandler.playerAspirant.selectedAttack = "Nothing";
            }
            else
            {
                // Deselect all other players
                foreach (PlayerObject player in phaseHandler.players)
                {
                    if (player != this)
                    {
                        player.isSelected = false;
                    }
                }

                phaseHandler.enemiesInRange = new HashSet<Vector2Int>();

                // Select this player
                isSelected = !isSelected;
                Debug.Log("Mouse Down on " + gameObject.name + ", isSelected: " + isSelected);

                if (isSelected)
                    phaseHandler.selectedPlayer = this;
                else
                    phaseHandler.selectedPlayer = null;
            }
        }

        else if (this.gameObject.CompareTag("Enemy"))
        {
            AiMovementLogic enemy = this.gameObject.GetComponent<AiMovementLogic>();
            Vector2Int enemyIndices = new Vector2Int(enemy.GetYIndex(), enemy.GetXIndex());

            if (phaseHandler.enemiesInRange.Contains(enemyIndices))
            {
                phaseHandler.selectedEnemy = this;

                if (phaseHandler.playerAspirant.selectedAttack == "SkillAttack")
                    phaseHandler.playerAspirant.SkillAttackDamage(phaseHandler);

                if (phaseHandler.playerAspirant.selectedAttack == "BasicAttack")
                {
                    phaseHandler.playerAspirant.BasicAttackDamage(phaseHandler);
                    phaseHandler.selectedPlayer.isSelected = false;
                    phaseHandler.selectedPlayer = null;
                }

                if (phaseHandler.playerAspirant.selectedAttack == "SignatureMoveAttack")
                    phaseHandler.playerAspirant.SignatureMoveAttackDamage(phaseHandler);
            }
        }
    }
}
