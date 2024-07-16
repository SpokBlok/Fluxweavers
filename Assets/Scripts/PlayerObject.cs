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
    public List<string> actionsUsed = new List<string>();
    public bool isSelected; //Check if the player is selected

    public bool isBasicAttackPhysical;

    public bool isSkillAttackPhysical;
    public bool skillAttackExists;
    public bool skillStatusExists;

    public bool isSignatureMoveAttackPhysical;
    public bool signatureMoveAttackExists;
    public bool signatureMoveStatusExists;

    public bool isMovementSkillActivated;

    //Mana & Resource Script
    public ResourceScript resourceScript;
    public int mana;

    // Phase Handler Script
    public PhaseHandler phaseHandler;

    // Sprites
    // to see if aspirant is selected or not
    [SerializeField] private Sprite normal;
    [SerializeField] private Sprite selected;

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
            if (phaseHandler.currentState == phaseHandler.playerAspirant)
            {
                // Deselect all other players
                foreach (PlayerObject player in phaseHandler.players)
                {
                    if (player != this)
                    {
                        player.isSelected = false;
                        player.GetComponent<SpriteRenderer>().sprite = normal;
                    }
                }

                phaseHandler.enemiesInRange = new HashSet<Vector2Int>();

                TogglePlayerSelection();

                Debug.Log("Mouse Down on " + gameObject.name + ", isSelected: " + isSelected);
            }
        }

        else if (this.gameObject.CompareTag("Enemy"))
        {
            AiMovementLogic enemy = this.GetComponent<AiMovementLogic>();
            Vector2Int enemyIndices = new Vector2Int(enemy.GetYIndex(), enemy.GetXIndex());

            if (phaseHandler.enemiesInRange.Contains(enemyIndices))
            {
                phaseHandler.selectedEnemy = this;
                phaseHandler.playerAspirant.BasicAttackDamage(phaseHandler);
                
                Debug.Log("Enemy is clicked!");

                // check if player was flagged to have moved already
                if (!phaseHandler.selectedPlayer.actionsUsed.Contains("movement"))
                {
                    AspirantMovement aspirant = phaseHandler.selectedPlayer.GetComponent<AspirantMovement>();

                    // if they are not in the position they are on in the beginning of the round
                    if (aspirant.currentXIndex != aspirant.originalXIndex ||
                        aspirant.currentYIndex != aspirant.originalYIndex)
                    {
                        // we can say that the player has chosen to lock in that move
                        phaseHandler.selectedPlayer.actionsUsed.Add("movement");

                        aspirant.originalXIndex = aspirant.currentXIndex;
                        aspirant.originalYIndex = aspirant.currentYIndex;
                    }
                }

                if (!phaseHandler.selectedPlayer.actionsUsed.Contains("ability"))
                    phaseHandler.selectedPlayer.actionsUsed.Add("ability");
            }
        }
    }

    public void TogglePlayerSelection()
    {
        // Select or Unselect this player
        isSelected = !isSelected;

        if (isSelected)
        {
            phaseHandler.selectedPlayer = this;
            GetComponent<SpriteRenderer>().sprite = selected;
        }

        else
        {
            AspirantMovement aspirant = GetComponent<AspirantMovement>();
            // if they are not in the position they are on in the beginning of the round
            if (aspirant.currentXIndex != aspirant.originalXIndex ||
                aspirant.currentYIndex != aspirant.originalYIndex)
            {
                aspirant.originalXIndex = aspirant.currentXIndex;
                aspirant.originalYIndex = aspirant.currentYIndex;
            }

            isMovementSkillActivated = false;
            phaseHandler.selectedPlayer = null;
            GetComponent<SpriteRenderer>().sprite = normal;
        }

        TilesCreationScript Tiles = GetComponent<AspirantMovement>().Tiles;
        if (Tiles.GetAdjacentTilesCount() > 0)
        {
            Tiles.HighlightAdjacentTiles(false);
            Tiles.SetAdjacentTiles(new HashSet<Vector2Int>());
        }
    }
}
