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
        if(Input.GetKeyDown(KeyCode.Space)) {
            ph.SwitchState(nextState);
        }
    }
}
