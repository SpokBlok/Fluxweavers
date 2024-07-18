using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseEnemyAspirant : PhaseBase
{
    PhaseBase nextState;
    public override void EnterState(PhaseHandler ph) {
        nextState = ph.roundEnd;
        
        ph.stateText.text = "Enemy Aspirant";
        ph.aiHandler.StartCoroutine(ph.aiHandler.MoveAi(ph.players));
    }

    public override void UpdateState(PhaseHandler ph) {
        if(Input.GetKeyDown(KeyCode.Space)) {
            ph.SwitchState(nextState);
        }
    }
}
