using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    public int shield;

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
    public bool hasMoved; // Check if the player has moved
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

    // Sprites
    // to see if aspirant is selected or not
    [SerializeField] private Sprite normal;
    [SerializeField] private Sprite selected;

    //Animation related things
    public Animator myAnimator;
    public GameObject splashArt;

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
            if (CompareTag("Player"))
            {
                StartCoroutine(DestroyObject());
                phaseHandler.playerPositions.Remove(this);
                phaseHandler.players.Remove(this);
            }
            else if (CompareTag("Enemy"))
            {
                phaseHandler.enemyPositions.Remove(this);
                phaseHandler.enemies.Remove(this);
            }
        }
    }

    public void IsAttacked(float opponentDamage)
    {  
        if (shield == 1) 
        {
            shield = 0;
        }

        else 
        {
            this.myAnimator.SetTrigger("HurtAnimation");
            health -= opponentDamage;
            IsDead();
        } 
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
        IsAttacked(125);

        if(this.gameObject.CompareTag("Player"))
        {   
            if (phaseHandler.currentState == phaseHandler.playerAspirant)
            {
                if(phaseHandler.playerAspirant.selectedAbility == "SkillAttack")
                {
                    phaseHandler.playerAspirant.SkillAttackDamage(phaseHandler);
                    phaseHandler.selectedPlayer.myAnimator.SetTrigger("SkillAttackUsed");
                    MoveCheck(phaseHandler.selectedPlayer);
                    phaseHandler.playerAspirant.selectedAbility = "Nothing";
                }

                else if(phaseHandler.playerAspirant.selectedAbility == "SignatureMoveAttack")
                {
                    phaseHandler.playerAspirant.SignatureMoveAttackDamage(phaseHandler);
                    phaseHandler.selectedPlayer.myAnimator.SetTrigger("SignatureMoveAttackUsed");
                    MoveCheck(phaseHandler.selectedPlayer);
                    phaseHandler.playerAspirant.selectedAbility = "Nothing";
                }

                else
                {
                    // Deselect currently selected player if there is any
                    if(phaseHandler.selectedPlayer != null)
                    {
                        if(phaseHandler.selectedPlayer != this)
                            phaseHandler.selectedPlayer.TogglePlayerSelection();
                    }

                    phaseHandler.alliesInRange = new HashSet<Vector2Int>();
                    phaseHandler.enemiesInRange = new HashSet<Vector2Int>();

                    TogglePlayerSelection();
                }
            }
            
        }

        else if (this.gameObject.CompareTag("Enemy"))
        {
            AiMovementLogic enemy = this.GetComponent<AiMovementLogic>();
            Vector2Int enemyIndices = new Vector2Int(enemy.GetYIndex(), enemy.GetXIndex());

            if (phaseHandler.enemiesInRange.Contains(enemyIndices))
            {
                phaseHandler.selectedEnemy = this;

                if (phaseHandler.playerAspirant.selectedAbility == "SkillAttack")
                {
                    phaseHandler.playerAspirant.SkillAttackDamage(phaseHandler);
                    phaseHandler.selectedPlayer.myAnimator.SetTrigger("SkillAttackUsed");
                    MoveCheck(phaseHandler.selectedPlayer);
                }

                if (phaseHandler.playerAspirant.selectedAbility == "BasicAttack")
                {
                    phaseHandler.selectedPlayer.myAnimator.SetTrigger("BasicAttackUsed");
                    phaseHandler.playerAspirant.BasicAttackDamage(phaseHandler);
                    MoveCheck(phaseHandler.selectedPlayer);
                }

                if (phaseHandler.playerAspirant.selectedAbility == "SignatureMoveAttack")
                {
                    phaseHandler.playerAspirant.SignatureMoveAttackDamage(phaseHandler);
                    phaseHandler.selectedPlayer.myAnimator.SetTrigger("SignatureMoveAttackUsed");
                    MoveCheck(phaseHandler.selectedPlayer);
                }
                
                Debug.Log("Enemy is clicked!");
            }
        }
    }

    public void MoveCheck(PlayerObject player)
    {
        // check if player was flagged to have moved already
        if (!player.hasMoved)
        {
            AspirantMovement aspirant = player.GetComponent<AspirantMovement>();

            // if they are not in the position they are on in the beginning of the round
            if (aspirant.currentXIndex != aspirant.originalXIndex ||
                aspirant.currentYIndex != aspirant.originalYIndex)
            {
                // we can say that the player has chosen to lock in that move
                player.hasMoved = true;

                aspirant.originalXIndex = aspirant.currentXIndex;
                aspirant.originalYIndex = aspirant.currentYIndex;
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
            
            phaseHandler.selectedPlayer = null;
            GetComponent<SpriteRenderer>().sprite = normal;
        }

        TilesCreationScript Tiles = GetComponent<AspirantMovement>().Tiles;
        if (Tiles.GetAdjacentTilesCount() > 0)
        {
            Tiles.HighlightAdjacentTiles(false);
            Tiles.SetAdjacentTiles(new HashSet<Vector2Int>());
        }

        phaseHandler.playerAspirant.selectedAbility = "none";
    }

    public IEnumerator SplashArtDisplay()
    {
        phaseHandler.selectedPlayer.splashArt.SetActive(true);
        yield return new WaitForSeconds(1);
        phaseHandler.selectedPlayer.splashArt.SetActive(false);
    }

    public void DisplaySplashArt()
    {
        StartCoroutine(SplashArtDisplay());
    }

    public IEnumerator DestroyObject()
    {
        AnimatorStateInfo stateInfo = myAnimator.GetCurrentAnimatorStateInfo(0);

        this.myAnimator.SetTrigger("DeathAnimation");
        yield return new WaitForEndOfFrame();
        while (stateInfo.IsName("DeathAnimation") == false)
        {
            yield return null;
            stateInfo = myAnimator.GetCurrentAnimatorStateInfo(0);
        }
        yield return new WaitForSeconds(stateInfo.length);
        Destroy(gameObject);
    }
}