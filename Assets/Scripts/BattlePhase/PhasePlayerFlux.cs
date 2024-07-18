using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhasePlayerFlux : PhaseBase
{
    PhaseBase nextState;
    public override void EnterState(PhaseHandler ph) {
        nextState = ph.playerAspirant;
        
        ph.stateText.text = "Player Flux";

        foreach (KeyValuePair<PlayerObject, Vector2Int> pair in ph.playerPositions)
        {
            PlayerObject entity = pair.Key;
            int x = pair.Value.x;
            int y = pair.Value.y;
            Hex occupiedHex = ph.tcs.Tiles[x, y].GetComponent<Hex>();
            occupiedHex.TerrainEffectRoundStart(entity);
        }

        foreach (KeyValuePair<PlayerObject, Vector2Int> pair in ph.enemyPositions)
        {
            PlayerObject entity = pair.Key;
            int x = pair.Value.x;
            int y = pair.Value.y;
            Hex occupiedHex = ph.tcs.Tiles[x, y].GetComponent<Hex>();
            occupiedHex.TerrainEffectRoundStart(entity);
        }
    }

    public override void UpdateState(PhaseHandler ph) {
        if(Input.GetKeyDown(KeyCode.Space)) {
            ph.SwitchState(nextState);
        }
    }
}
