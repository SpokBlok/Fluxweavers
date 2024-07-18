using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseMatchStart : PhaseBase
{
    PhaseBase nextState;
    public override void EnterState(PhaseHandler ph) {
        nextState = ph.playerFlux;
        
        ph.stateText.text = "Match Start";
    }

    public override void UpdateState(PhaseHandler ph) {
        if(Input.GetKeyDown(KeyCode.Space)) {
            ph.SwitchState(nextState);
        }
    }
}
