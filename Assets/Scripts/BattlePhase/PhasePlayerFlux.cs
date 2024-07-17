using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhasePlayerFlux : PhaseBase
{
    PhaseBase nextState;
    public override void EnterState(PhaseHandler ph) {
        nextState = ph.playerAspirant;
        
        ph.stateText.text = "Player Flux";
    }

    public override void UpdateState(PhaseHandler ph) {
        if(Input.GetKeyDown(KeyCode.Space)) {
            ph.SwitchState(nextState);
        }
    }
}
