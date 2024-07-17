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
        
        if (ph.currentRound%2==0) 
            nextState = ph.playerFlux;
        else
            nextState = ph.enemyFlux;

        //Tile effects for end of round shenanigans
        foreach((PlayerObject, int, int) tuple in ph.entityPositions) {
            PlayerObject entity = tuple.Item1;
            int yPos = tuple.Item2;
            int xPos = tuple.Item3;
            Hex occupiedHex = ph.tcs.Tiles[yPos,xPos].GetComponent<Hex>();
            occupiedHex.TerrainEffect(entity);
        }
        
        onRoundEnd?.Invoke();
    }

    public override void UpdateState(PhaseHandler ph) {
        if(Input.GetKeyDown(KeyCode.Space)) {
            ph.SwitchState(nextState);
        }
    }
}
