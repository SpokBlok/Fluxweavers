using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
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

        List<PlayerObject> playerKeys = new List<PlayerObject>(ph.playerPositions.Keys);
        List<PlayerObject> enemyKeys = new List<PlayerObject>(ph.enemyPositions.Keys);

        foreach (PlayerObject key in playerKeys)
        {
            Vector2Int index = ph.playerPositions[key];
            Hex occupiedHex = ph.tcs.Tiles[index.x, index.y].GetComponent<Hex>();
            occupiedHex.TerrainEffectRoundEnd(key);
        }

        foreach (PlayerObject key in enemyKeys)
        {
            Vector2Int index = ph.enemyPositions[key];
            Hex occupiedHex = ph.tcs.Tiles[index.x, index.y].GetComponent<Hex>();
            occupiedHex.TerrainEffectRoundEnd(key);
        }

        onRoundEnd?.Invoke();


    }

    public override void UpdateState(PhaseHandler ph) {
        if(Input.GetKeyDown(KeyCode.Space)) {
            ph.SwitchState(nextState);
        }
    }
}
