using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhasePlayerFlux : PhaseBase
{
    PhaseBase nextState;
    public override void EnterState(PhaseHandler ph) {
        nextState = ph.playerAspirant;
        
        ph.stateText.text = "Player Flux";

        List<PlayerObject> playerKeys = new List<PlayerObject>(ph.playerPositions.Keys);
        List<PlayerObject> enemyKeys = new List<PlayerObject>(ph.enemyPositions.Keys);

        foreach (PlayerObject key in playerKeys)
        {
            Vector2Int index = ph.playerPositions[key];
            Hex occupiedHex = ph.tcs.Tiles[index.x, index.y].GetComponent<Hex>();
            occupiedHex.TerrainEffectRoundStart(key);
        }

        foreach (PlayerObject key in enemyKeys)
        {
            Vector2Int index = ph.enemyPositions[key];
            Hex occupiedHex = ph.tcs.Tiles[index.x, index.y].GetComponent<Hex>();
            occupiedHex.TerrainEffectRoundStart(key);
        }
    }

    public override void UpdateState(PhaseHandler ph) {
        if(Input.GetKeyDown(KeyCode.Space)) {
            ph.SwitchState(nextState);
        }
    }
}
