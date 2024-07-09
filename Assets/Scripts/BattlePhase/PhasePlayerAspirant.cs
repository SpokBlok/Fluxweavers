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

    public override void UpdateState(PhaseHandler ph) {

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (ph.rs.playerAbilityUseCheck(ph.player.basicAttackMana))
            {
                //check for damagetype
                //if phys
                    //damage = player.basicAttack(enemyArmor)
                //if magic
                    //damage = player.basicAttack(enemyMagRes)

                //enemy.health = enemy.health - damage
            }

            ph.player.basicAttack();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (ph.rs.playerAbilityUseCheck(ph.player.skillMana))
            {

                //check for skill type
                    //if attack
                        //check for damagetype
                            //if phys
                        //damage = player.basicAttack(enemyArmor)
                            //if magic
                        //damage = player.basicAttack(enemyMagRes)
                    //if buff/debuff
                        //send to manager
            }
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            if (ph.rs.playerAbilityUseCheck(ph.player.signatureMoveMana))
            {
                //damage = player.signatureAttack(enemyArmor)
                //enemy.health = enemy.health - damage
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            ph.SwitchState(nextState);
        }
    }
}
