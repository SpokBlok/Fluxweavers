using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    //Player Stats
    public int level;
    //public health Health;
    public float damageResist;
    public float fluxResist;
    public int attackStat;
    public int movement;
    public int vision; // All players have a vision of 2 hexes

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

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        vision = 2;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
