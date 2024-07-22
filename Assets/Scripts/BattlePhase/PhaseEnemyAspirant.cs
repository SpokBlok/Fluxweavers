using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseEnemyAspirant : PhaseBase
{
    PhaseBase nextState;
    public override void EnterState(PhaseHandler ph) {
        nextState = ph.roundEnd;
        ph.stateText.text = "Enemy Aspirant";
        Debug.Log(ph.players.Count);
        Coroutine co = ph.aiHandler.StartCoroutine(ph.aiHandler.MoveAi(ph.players));
        if (ph.players.Count == 0)
        {
            ph.aiHandler.StopCoroutine(co);
        }
    }

    public override void UpdateState(PhaseHandler ph) {
        if(Input.GetKeyDown(KeyCode.Space)) {
            ph.SwitchState(nextState);
        }
    }
}
