using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raccoon : PlayerObject
{
    // Start is called before the first frame update
    HashSet<PlayerObject> raccoon;

    void Start()
    {
        raccoon = new HashSet<PlayerObject>() {this};
        level = 1;
        health = 300;

        attackStat = 20;
        armor = 10;
        magicResistance = 8;
        armorPenetration = 0;
        magicPenetration = 0;
        movement = 2;
        control = 2;

        basicAttackMana = 4;
        basicAttackRange = 1;

        skillMana = 7;
        skillRange = 0;
    }

    public override float basicAttack(float armor, float enemyCurrentHealth, float enemyMaximumHealth)
    {
        if (resourceScript.playerAbilityUseCheck(basicAttackMana)) {
            resourceScript.playerAbilityUseManaUpdate(basicAttackMana);
            
            float damageOutput = attackStat * 1.5f / armor;
            return damageOutput;
        }

        Debug.Log("Unable to basic attack");
        return 0; // Basic Attack not done;
    }

    public override void skillStatus(HashSet<PlayerObject> targets)
    {
        if (resourceScript.playerAbilityUseCheck(skillMana) == true) {
            StatusEffect effect = new();
            StatusEffectHandlerScript Handler = GameObject.FindGameObjectWithTag("StatusEffectHandler").GetComponent<StatusEffectHandlerScript>();

            effect.instantiateMultiFloatEffect("attackStat", 1.5f, 2, raccoon);
            Handler.addStatusEffect(effect);
        }
    }
}
