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
    public float basicAttackRange;
    public bool isBasicAttackPhysical;

    public float skillDamage;
    public int skillMana;
    public float skillRange;

    public float signatureMoveDamage;
    public int signatureMoveMana;
    public float signatureMoveRange;

    // Checkers
    public bool hasMoved; // Check if the player has moved in that turn
    public bool isSelected; //Check if the player is selected

    //Mana & Resource Script
    public ResourceScript resourceScript;
    public int mana;

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        isSelected = false;
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

    public void IsAttacked(float opponentDamage, float opponentAttackStat, float opponentArmorPenetration, float opponentMagicPenetration, bool isPhysical)
    {
        if (isPhysical)
            health = health - healthBar.TotalDamageReceived(opponentDamage, opponentAttackStat, armor, opponentArmorPenetration);
        else
            health = health - healthBar.TotalDamageReceived(opponentDamage, opponentAttackStat, magicResistance, opponentMagicPenetration);
        IsDead();
    }

    public virtual float basicAttack(float armor)
    {
        return 0;
    }

    public virtual float skillAttack()
    {
        return 0;
    }

    public virtual float skillStatus()
    {
        return 0;
    }


    public virtual float signatureMove()
    {
        return 0;
    }

    public void OnMouseDown()
    {
        // Deselect all other players
        PlayerObject[] allPlayers = FindObjectsOfType<PlayerObject>();
        foreach (PlayerObject player in allPlayers)
        {
            if (player != this)
            {
                player.isSelected = false;
            }
        }

        // Select this player
        isSelected = !isSelected;
        Debug.Log("Mouse Down on " + gameObject.name + ", isSelected: " + isSelected);
    }
}
