using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhasePlayerAspirant : PhaseBase
{
    PhaseBase nextState;

    public override void EnterState(PhaseHandler ph) {
        if(ph.currentRound%2==0) 
            nextState = ph.enemyAspirant;
        else
            nextState = ph.roundEnd;
        
        ph.stateText.text = "Player Aspirant";
    }

    public override void UpdateState(PhaseHandler ph)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ph.SwitchState(nextState);
        }

        foreach (var player in ph.players)
        {
            if (player.isSelected)
            {
                if (Input.GetKeyDown(KeyCode.B))
                {
                    Debug.Log("B was Pressed");
                }
                {
                    if (ph.rs.playerAbilityUseCheck(player.basicAttackMana))
                    {
                        float damage = 0;
                        // Check for damage type
                        if (player.isBasicAttackPhysical)
                            damage = player.basicAttack(ph.enemy.armor);
                        else
                            damage = player.basicAttack(ph.enemy.magicResistance);

                        ph.enemy.health = ph.enemy.health - damage;
                    }
                }

                if (Input.GetKeyDown(KeyCode.S))
                {
                    if (ph.rs.playerAbilityUseCheck(player.skillMana))
                    {
                        // Check for skill type
                        // if attack
                        // check for damage type
                        // if phys
                        // damage = player.basicAttack(enemyArmor)
                        // if magic
                        // damage = player.basicAttack(enemyMagRes)
                        // if buff/debuff
                        // send to manager
                    }
                }

                if (Input.GetKeyDown(KeyCode.U))
                {
                    if (ph.rs.playerAbilityUseCheck(player.signatureMoveMana))
                    {
                        // damage = player.signatureAttack(enemyArmor)
                        // enemy.health = enemy.health - damage
                    }
                }
            }
        }
    }
}
