using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public int health;
    public bool isDead;

    void Start()
    {
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetHealth(int setHealth)
    {
        health = setHealth;
    }

    public void TotalDamageReceived(int damage, int attackStat, int resistStat, int penetrationStat) // How much damage the player receives. ResistStat comes from you, and the other stats come from the damage dealer.
    {
        damage = damage / 100; //Converting damage to be multiplied by percentage of attack as seen in the aspirant stats
        int totalResist = resistStat - penetrationStat; 
        health -= (damage * attackStat) / totalResist;
    }
}