using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PhaseRoundEnd : PhaseBase
{
    PhaseBase nextState;

    public override void EnterState(PhaseHandler ph) {
        ph.currentRound += 1;

        ph.player.manaRoundStart(ph.rs.roundStart(ph.currentRound));
        ph.enemy.manaRoundStart(ph.rs.roundStart(ph.currentRound));

        if (ph.currentRound%2==0) 
            nextState = ph.playerFlux;
        else
            nextState = ph.enemyFlux;
        
        ph.stateText.text = "Round End";


    }

    public override void UpdateState(PhaseHandler ph) {
        if(Input.GetKeyDown(KeyCode.Space)) {
            ph.SwitchState(nextState);
        }
    }
}
