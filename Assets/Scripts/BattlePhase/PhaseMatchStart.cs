using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseMatchStart : PhaseBase
{
    PhaseBase nextState;
    public override void EnterState(PhaseHandler ph) {
        if(ph.currentRound%2==0) 
            nextState = ph.playerFlux;
        else
            nextState = ph.enemyFlux;
        
        ph.stateText.text = "Match Start";
    }

    public override void UpdateState(PhaseHandler ph) {
        if(Input.GetKeyDown(KeyCode.Space)) {
            ph.SwitchState(nextState);
        }
    }
}
