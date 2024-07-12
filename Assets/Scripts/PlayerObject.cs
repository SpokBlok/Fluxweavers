using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    //Player Stats
    public int level;
    public float armor;
    public float armorPenetration;
    public float magicResistance;
    public float magicPenetration;
    public float attackStat;
    public int movement;
    public int control; // All players have control over 2 hexes

    // Health Related Stuff
    public HealthBar healthBar;
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
    public bool isBasicAttackPhysical;

    public bool isSkillAttackPhysical;
    public bool skillAttackExists;
    public bool skillStatusExists;

    public bool isSignatureMoveAttackPhysical;
    public bool signatureMoveAttackExists;
    public bool signatureMoveStatusExists;

    //Mana & Resource Script
    public ResourceScript resourceScript;
    public int mana;

    // Phase Handler Script
    public PhaseHandler phaseHandler;

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
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

    public virtual float basicAttack(float armor)
    {
        return 0;
    }

    public virtual float skillAttack(float enemyResistStat, float enemyCurrentHealth, float enemyMaximumHealth)
    {
        return 0;
    }

    public virtual void skillStatus()
    {
    }

    public virtual float signatureMoveAttack(int enemyResistStat)
    {
        return 0;
    }

    public virtual void signatureMoveStatus()
    {
    }
    public void OnMouseDown()
    {
        if(this.gameObject.CompareTag("Player"))
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

        else if (this.gameObject.CompareTag("Enemy"))
        {
            AiMovementLogic enemy = this.gameObject.GetComponent<AiMovementLogic>();
            Vector2Int enemyIndices = new Vector2Int(enemy.GetYIndex(), enemy.GetXIndex());

            if (phaseHandler.enemiesInRange.Contains(enemyIndices))
            {
                phaseHandler.selectedEnemy = this;
                phaseHandler.playerAspirant.BasicAttackDamage(phaseHandler);
                
                Debug.Log("Enemy is clicked!");
            }
        }
    }
}
