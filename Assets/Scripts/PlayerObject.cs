using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    //Player Stats
    public int level;
    //public health Health;
    public float armor;
    public float armorPenetration;
    public float magicResistance;
    public float magicPenetration;
    public float fluxResist;
    public float attackStat;
    public int movement;
    public int control; // All players have control over 2 hexes

    //Attack Stats
    public float basicAttackDamage;
    public float basicAttackMana;
    public float basicAttackRange;

    public float skillDamage;
    public float skillMana;
    public float skillRange;

    public float signatureMoveDamage;
    public float signatureMoveMana;
    public float signatureMoveRange;

    // Checkers
    public bool hasMoved; // Check if the player has moved in that turn

    //Mana & Resource Script
    public ResourceScript rs;
    public int mana;

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
}
