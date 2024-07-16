using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseEnemyAspirant : PhaseBase
{
    PhaseBase nextState;
    public override void EnterState(PhaseHandler ph) {
        if(ph.currentRound%2==0) 
            nextState = ph.roundEnd;
        else
            nextState = ph.playerAspirant;
        
        ph.stateText.text = "Enemy Aspirant";
        ph.aiHandler.StartCoroutine(ph.aiHandler.MoveAi());
    }

    public override void UpdateState(PhaseHandler ph) {
        if(Input.GetKeyDown(KeyCode.Space)) {
            ph.SwitchState(nextState);
        }
    }
}
