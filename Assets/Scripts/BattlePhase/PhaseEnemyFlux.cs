using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseEnemyFlux : PhaseBase
{
    PhaseBase nextState;
    public override void EnterState(PhaseHandler ph) {
        if(ph.currentRound%2==0) 
            nextState = ph.playerAspirant;
        else
            nextState = ph.playerFlux;
        
        ph.stateText.text = "Enemy Flux";
    }

    public override void UpdateState(PhaseHandler ph) {
        if(Input.GetKeyDown(KeyCode.Space)) {
            ph.SwitchState(nextState);
        }
    }
}
