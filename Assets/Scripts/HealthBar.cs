using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//probably gonna delete this
public class HealthBar : MonoBehaviour
{
    public float totalDamage;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public float TotalDamageReceived(float damage, float attackStat, float resistStat, float penetrationStat) // How much damage the player receives. ResistStat comes from you, and the other stats come from the damage dealer.
    {
        totalDamage = 0;
        damage = damage / 100; //Converting damage to be multiplied by percentage of attack as seen in the aspirant stats
        float totalResist = resistStat - penetrationStat; 
        totalDamage += (damage * attackStat) / totalResist;
        return totalDamage;
    }
}