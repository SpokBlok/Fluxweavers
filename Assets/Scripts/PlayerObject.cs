using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

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

    // Dedra Animations
    public string objectName;
    public bool isSkillActive; // checks if skill is still active (should last only 3 turns)
    public bool isSignatureMoveActive; // checks if signature move was activated
    public int skillCounter;
    public int signatureMoveCounter;
    public float calculatedBasicAttackDamage;
    public float preCalculatedBasicAttackDamage;

    // to see if aspirant is selected or not
    [SerializeField] private Sprite normal;
    [SerializeField] private Sprite selected;

    //Animation related things
    public Animator myAnimator;
    public GameObject splashArt;

    public AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        maxHealth = health;
        attackStat = 20;
        calculatedBasicAttackDamage = 0;
        
        //Voicelines
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (phaseHandler.selectedPlayer != null)
        {
            if (phaseHandler.selectedPlayer.objectName == "Dedra")
                {
                    if (phaseHandler.selectedPlayer.isSkillActive && phaseHandler.selectedPlayer.isSignatureMoveActive)
                    {
                        phaseHandler.selectedPlayer.myAnimator.SetTrigger("Idle_Combo");
                    }

                    else if (phaseHandler.selectedPlayer.isSkillActive)
                    {
                        phaseHandler.selectedPlayer.myAnimator.SetTrigger("Idle_Skill");
                    }

                    else if (phaseHandler.selectedPlayer.isSignatureMoveActive)
                    {
                        phaseHandler.selectedPlayer.myAnimator.SetTrigger("Idle_Ultimate");
                    }
                }
        }
    }

    //Adds healths
    public void AddHealth(float healing){
        health += healing;
        if(health > maxHealth)
            health = maxHealth;
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
                StartCoroutine(DestroyObject());
                phaseHandler.enemyPositions.Remove(this);
                phaseHandler.enemies.Remove(this);
            }
        }
    }

    public virtual void IsAttacked(float opponentDamage)
    {
        if (shield == 1)
        {
            shield = 0;
        }

        else
        {
            try
            {
                if (phaseHandler.selectedPlayer != null)
                {
                    if (phaseHandler.selectedPlayer.objectName == "Dedra")
                    {
                        if (phaseHandler.selectedPlayer.isSkillActive && phaseHandler.selectedPlayer.isSignatureMoveActive)
                        {
                            this.myAnimator.SetTrigger("SkillHurt");
                        }

                        else if (phaseHandler.selectedPlayer.isSkillActive && !phaseHandler.selectedPlayer.isSignatureMoveActive)
                        {
                            this.myAnimator.SetTrigger("SkillHurt");
                        }

                        else if (phaseHandler.selectedPlayer.isSignatureMoveActive && !phaseHandler.selectedPlayer.isSkillActive)
                        {
                            this.myAnimator.SetTrigger("HurtAnimation");
                        }

                        else if (!phaseHandler.selectedPlayer.isSkillActive && !phaseHandler.selectedPlayer.isSignatureMoveActive)
                        {
                            this.myAnimator.SetTrigger("HurtAnimation");
                        }
                    }
                    else
                    {
                        this.myAnimator.SetTrigger("HurtAnimation");
                    }
                }
                this.myAnimator.SetTrigger("HurtAnimation");
                health -= opponentDamage;
                IsDead();
            }

            catch
            {
                this.myAnimator.SetTrigger("HurtAnimation");
                health -= opponentDamage;
                IsDead();
            }
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

    public virtual bool isMeetingFluxAffinity()
    {
        return false;
    }

    public void OnMouseDown()
    {    
        if(this.gameObject.CompareTag("Player"))
        {   
            if (phaseHandler.currentState == phaseHandler.playerAspirant)
            {
                if(phaseHandler.playerAspirant.selectedAbility == "Skill")
                {
                    //Checks if Aspirant is Citrine or Maiko, then assigns Skill animation
                    if(phaseHandler.selectedPlayer.objectName == "Citrine")
                    {
                        audioManager.PlaySFX(audioManager.citrineSkill);
                        phaseHandler.selectedPlayer.myAnimator.SetTrigger("SkillAttackUsed");
                    }
                    else if(phaseHandler.selectedPlayer.objectName == "Maiko")
                    {
                        audioManager.PlaySFX(audioManager.maikoSkill);
                        phaseHandler.selectedPlayer.myAnimator.SetTrigger("SkillAttackUsed");
                    }
                    //If aspirant is Dedra, go through her various statuses
                    else if(phaseHandler.selectedPlayer.objectName == "Dedra")
                    {
                        audioManager.PlaySFX(audioManager.dedraSkill);
                        if (phaseHandler.selectedPlayer.isSignatureMoveActive)
                        {
                            phaseHandler.selectedPlayer.myAnimator.SetTrigger("Combo_UltimateThenSkill");
                        }
                        else
                        {
                            phaseHandler.selectedPlayer.myAnimator.SetTrigger("SkillAttackUsed");
                        }
                    }

                    //Skill & and MoveCheck gets triggered
                    phaseHandler.playerAspirant.SkillAttackDamage(phaseHandler);
                    MoveCheck(phaseHandler.selectedPlayer);
                }

                else if(phaseHandler.playerAspirant.selectedAbility == "SignatureMove")
                {
                    //Checks if Aspirant is Citrine or Maiko, then assigns Signature animation
                    if(phaseHandler.selectedPlayer.objectName == "Citrine")
                    {
                        audioManager.PlaySFX(audioManager.citrineUltCast);
                        phaseHandler.selectedPlayer.myAnimator.SetTrigger("SignatureMoveAttackUsed");
                    }
                    else if(phaseHandler.selectedPlayer.objectName == "Maiko")
                    {
                        // audioManager.PlaySFX(audioManager.maikoUltActivation);
                        audioManager.PlaySFX(audioManager.maikoUltCast);
                        phaseHandler.selectedPlayer.myAnimator.SetTrigger("SignatureMoveAttackUsed");                        
                    }
                    //If aspirant is Dedra, go through her various statuses
                    else if(phaseHandler.selectedPlayer.objectName == "Dedra")
                    {
                        audioManager.PlaySFX(audioManager.dedraUltCast);
                        if (phaseHandler.selectedPlayer.isSkillActive)
                        {
                            phaseHandler.selectedPlayer.myAnimator.SetTrigger("Combo_SkillThenUltimate");
                        }

                        else
                        {
                        phaseHandler.selectedPlayer.myAnimator.SetTrigger("SignatureMoveAttackUsed");
                        }
                    }

                    //Signature Move & and MoveCheck gets triggered
                    phaseHandler.playerAspirant.SignatureMoveAttackDamage(phaseHandler);
                    MoveCheck(phaseHandler.selectedPlayer);
                }
                else
                {
                    // Deselect currently selected player if there is any
                    if(phaseHandler.selectedPlayer != null && phaseHandler.selectedPlayer != this)
                    {
                        phaseHandler.selectedPlayer.TogglePlayerSelection();

                        AspirantInterface aspirantUI = phaseHandler.playerAspirant.aspirantUI;
                        aspirantUI.tooltip.SetActive(false);
                            

                        if (phaseHandler.playerAspirant.selectedAbility != "none")
                            aspirantUI.ToggleAbilityButton(false);
                    }

                    phaseHandler.alliesInRange = new HashSet<Vector2Int>();
                    phaseHandler.enemiesInRange = new HashSet<Vector2Int>();

                    TogglePlayerSelection();
                }
            }
            
        }

        else if (this.gameObject.CompareTag("Enemy"))
        {

            Vector2Int enemyIndices = new Vector2Int(0, 0);
            try
            {
                AiMovementLogic enemy = this.GetComponent<AiMovementLogic>();   
                enemyIndices = new Vector2Int(enemy.GetYIndex(), enemy.GetXIndex());
            }
            catch (Exception)
            {
                Nexus enemyNexus = this.GetComponent<Nexus>();
                enemyIndices = new Vector2Int(enemyNexus.y, enemyNexus.x);
            }

            if (phaseHandler.enemiesInRange.Contains(enemyIndices))
            {
                phaseHandler.selectedEnemy = this;

                if (phaseHandler.playerAspirant.selectedAbility == "Skill")
                {
                    if(phaseHandler.selectedPlayer.objectName == "Citrine")
                    {
                        audioManager.PlaySFX(audioManager.citrineSkill);
                    }
                    else if(phaseHandler.selectedPlayer.objectName == "Dedra")
                    {
                        audioManager.PlaySFX(audioManager.dedraSkill);
                    }
                    else if(phaseHandler.selectedPlayer.objectName == "Maiko")
                    {
                        audioManager.PlaySFX(audioManager.maikoSkill);
                    }
                    phaseHandler.playerAspirant.SkillAttackDamage(phaseHandler);
                    phaseHandler.selectedPlayer.myAnimator.SetTrigger("SkillAttackUsed");
                    MoveCheck(phaseHandler.selectedPlayer);
                }

                if (phaseHandler.playerAspirant.selectedAbility == "BasicAttack")
                {
                    //Checks if Aspirant is Citrine or Maiko, then assigns Basic Attack animation
                    if(phaseHandler.selectedPlayer.objectName == "Citrine")
                    {
                        audioManager.PlaySFX(audioManager.citrineBasicAttack);
                        phaseHandler.selectedPlayer.myAnimator.SetTrigger("BasicAttackUsed");
                    }
                    else if(phaseHandler.selectedPlayer.objectName == "Maiko")
                    {
                        //audioManager.PlaySFX(audioManager.maikoBasicAttack);
                        phaseHandler.selectedPlayer.myAnimator.SetTrigger("BasicAttackUsed");                        
                    }
                    //If aspirant is Dedra, go through her various statuses
                    else if(phaseHandler.selectedPlayer.objectName == "Dedra")
                    {
                        audioManager.PlaySFX(audioManager.dedraBasicAttack);
                        if (phaseHandler.selectedPlayer.isSkillActive && phaseHandler.selectedPlayer.isSignatureMoveActive)
                            { 
                                preCalculatedBasicAttackDamage = phaseHandler.selectedPlayer.basicAttack(phaseHandler.selectedEnemy.armor, phaseHandler.selectedEnemy.health,phaseHandler.selectedEnemy.maxHealth);
                                if (preCalculatedBasicAttackDamage >= phaseHandler.selectedEnemy.health)
                                {
                                    if (phaseHandler.selectedPlayer.skillCounter <= 1)
                                    {
                                        phaseHandler.selectedPlayer.myAnimator.SetTrigger("ComboLastShotForSkill");
                                    }

                                    else
                                    {
                                        phaseHandler.selectedPlayer.myAnimator.SetTrigger("ComboBasicAttack");
                                    }
                                }
                                
                                else
                                {
                                    if (phaseHandler.selectedPlayer.skillCounter <= 1)
                                    {
                                        phaseHandler.selectedPlayer.myAnimator.SetTrigger("ComboLastShotForBoth");
                                    }

                                    else
                                    {
                                    phaseHandler.selectedPlayer.myAnimator.SetTrigger("ComboLastShotForUltimate");
                                    }
                                }
                            }
                            
                            // basic attack if skill is active
                            else if (phaseHandler.selectedPlayer.isSkillActive)
                            {
                                if (phaseHandler.selectedPlayer.skillCounter == 1)
                                {
                                    phaseHandler.selectedPlayer.myAnimator.SetTrigger("Skill_LastShot");
                                }

                                else 
                                {
                                    phaseHandler.selectedPlayer.myAnimator.SetTrigger("SkillBasicAttack");
                                }
                            }

                            // basic attack if signature move is active
                            else if (phaseHandler.selectedPlayer.isSignatureMoveActive)
                            {
                                preCalculatedBasicAttackDamage = phaseHandler.selectedPlayer.basicAttack(phaseHandler.selectedEnemy.armor, phaseHandler.selectedEnemy.health,phaseHandler.selectedEnemy.maxHealth);

                                if (preCalculatedBasicAttackDamage >= phaseHandler.selectedEnemy.health)
                                {
                                    phaseHandler.selectedPlayer.myAnimator.SetTrigger("Ultimate_BasicAttack");
                                }

                                else
                                {
                                    phaseHandler.selectedPlayer.myAnimator.SetTrigger("Ultimate_LastShot");
                                }
                            }
                            
                            // basic attack if neither skill nor signature move are active
                            else if (!phaseHandler.selectedPlayer.isSkillActive && !phaseHandler.selectedPlayer.isSignatureMoveActive)
                            { 
                                phaseHandler.selectedPlayer.myAnimator.SetTrigger("BasicAttackUsed");
                            }
                    }

                    phaseHandler.playerAspirant.BasicAttackDamage(phaseHandler);
                    MoveCheck(phaseHandler.selectedPlayer);
                }

                if (phaseHandler.playerAspirant.selectedAbility == "SignatureMove")
                {
                    if(phaseHandler.selectedPlayer.objectName == "Citrine")
                    {
                        audioManager.PlaySFX(audioManager.citrineUltCast);
                    }
                    else if(phaseHandler.selectedPlayer.objectName == "Dedra")
                    {
                        audioManager.PlaySFX(audioManager.dedraUltCast);
                    }
                    else if(phaseHandler.selectedPlayer.objectName == "Maiko")
                    {
                        audioManager.PlaySFX(audioManager.maikoUltCast);
                    }
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
        if (!player.hasMoved && !player.objectName.Equals("Nexus"))
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
            phaseHandler.playerAspirant.aspirantUI.SetupButtonsAndImages();
        }

        else
        {
            AspirantMovement aspirant = GetComponent<AspirantMovement>();
            
            phaseHandler.selectedPlayer = null;
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
        //Voicelines
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        Debug.Log("AudioManager initialized: " + (audioManager != null));
        if (phaseHandler.selectedPlayer.objectName == "Maiko")
        {
            audioManager.PlaySFX(audioManager.maikoUltActivation);
        }
        else if (phaseHandler.selectedPlayer.objectName == "Citrine")
        {
            audioManager.PlaySFX(audioManager.citrineUltActivation);
        }
        if (phaseHandler.selectedPlayer.objectName == "Dedra")
        {
            audioManager.PlaySFX(audioManager.dedraUltActivation);
        }
        StartCoroutine(SplashArtDisplay());
    }

    public IEnumerator DestroyObject()
    {
        AnimatorStateInfo stateInfo = myAnimator.GetCurrentAnimatorStateInfo(0);

/*        if (phaseHandler.selectedPlayer != null)
        {
            if (phaseHandler.selectedPlayer.objectName == "Dedra")
            {
                if (phaseHandler.selectedPlayer.isSkillActive && phaseHandler.selectedPlayer.isSignatureMoveActive)
                {
                    this.myAnimator.SetTrigger("Skill_Death");
                    yield return new WaitForEndOfFrame();

                    while (stateInfo.IsName("Skill_Death") == false)
                    {
                        yield return null;
                        stateInfo = myAnimator.GetCurrentAnimatorStateInfo(0);
                    }
                    
                    yield return new WaitForSeconds(stateInfo.length);
                    Destroy(gameObject);
                }

                else if (phaseHandler.selectedPlayer.isSkillActive && !phaseHandler.selectedPlayer.isSignatureMoveActive)
                {
                    this.myAnimator.SetTrigger("Skill_Death");
                    yield return new WaitForEndOfFrame();

                    while (stateInfo.IsName("Skill_Death") == false)
                    {
                        yield return null;
                        stateInfo = myAnimator.GetCurrentAnimatorStateInfo(0);
                    }
                    
                    yield return new WaitForSeconds(stateInfo.length);
                    Destroy(gameObject);
                }

                else if (phaseHandler.selectedPlayer.isSignatureMoveActive && !phaseHandler.selectedPlayer.isSkillActive)
                {
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

                else if (!phaseHandler.selectedPlayer.isSkillActive && !phaseHandler.selectedPlayer.isSignatureMoveActive)
                {
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
            }*/

            
            
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
