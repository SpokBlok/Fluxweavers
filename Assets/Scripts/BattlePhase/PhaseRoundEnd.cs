using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PhaseRoundEnd : PhaseBase
{
    //The two functions define an event thats invoked everytime round end phase starts
    public delegate void RoundEnd();
    public static event RoundEnd onRoundEnd;
    PhaseBase nextState;
    public override void EnterState(PhaseHandler ph) {

        ph.currentRound += 1;
        ph.rs.roundStart(ph.currentRound);
        ph.stateText.text = "Round End";
        
        nextState = ph.playerFlux;
        
        onRoundEnd?.Invoke();
    }

    public override void UpdateState(PhaseHandler ph) {
        if(Input.GetKeyDown(KeyCode.Space)) {
            ph.SwitchState(nextState);
        }
    }
}
