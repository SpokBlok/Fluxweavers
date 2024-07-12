using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhasePlayerAspirant : PhaseBase
{
    PhaseBase nextState;
    private TilesCreationScript tiles; 
    private HashSet<Vector2Int> availableTiles;

    public override void EnterState(PhaseHandler ph)
    {
        if (ph.currentRound % 2 == 0)
            nextState = ph.enemyAspirant;
        else
            nextState = ph.roundEnd;

        ph.stateText.text = "Player Aspirant";

        // Initialize availableTiles HashSet
        availableTiles = new HashSet<Vector2Int>();

        // Find TilesCreationScript in the scene
        tiles = GameObject.FindObjectOfType<TilesCreationScript>();
        if (tiles == null)
        {
            Debug.LogError("TilesCreationScript not found in the scene.");
        }
    }

    public override void UpdateState(PhaseHandler ph)
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (ph.rs.playerAbilityUseCheck(ph.selectedPlayer.basicAttackMana))
            {
                float damage = 0;
                // Check for damage type
                if (ph.selectedPlayer.isBasicAttackPhysical)
                    damage = ph.selectedPlayer.basicAttack(ph.enemy.armor);
                else
                    damage = ph.selectedPlayer.basicAttack(ph.enemy.magicResistance);

                ph.enemy.health = ph.enemy.health - damage;

                // Get current position indices from AspirantMovement script
                AspirantMovement aspirantMovement = ph.selectedPlayer.GetComponent<AspirantMovement>();
                if (aspirantMovement != null)
                {
                    int currentXIndex = aspirantMovement.currentXIndex;
                    int currentYIndex = aspirantMovement.currentYIndex;

                    availableTiles = GetAdjacentTiles(currentXIndex, currentYIndex, (int)ph.selectedPlayer.basicAttackRange);

                    // un-highlight the previous adjacent tiles if there are any
                    if(tiles.GetAdjacentTilesCount() > 0)
                        tiles.HighlightAdjacentTiles(false);

                    // set the new adjacent tiles and highlight them
                    tiles.SetAdjacentTiles(availableTiles);
                    tiles.HighlightAdjacentTiles(true);
                }
                else
                {
                    Debug.LogError("AspirantMovement script not found on PlayerObject.");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (ph.rs.playerAbilityUseCheck(ph.selectedPlayer.skillMana))
            {
                // Handle skill logic
                // Placeholder: Insert your skill logic here
            }
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            if (ph.rs.playerAbilityUseCheck(ph.selectedPlayer.signatureMoveMana))
            {
                // Handle signature move logic
                // Placeholder: Insert your signature move logic here
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ph.SwitchState(nextState);
        }
    }

    HashSet<Vector2Int> GetAdjacentTiles(int xIndex, int yIndex, int range)
    {
        HashSet<Vector2Int> AdjacentTiles = new HashSet<Vector2Int>();
        AdjacentTiles.Add(new Vector2Int(yIndex, xIndex));

        // setting up steps from current tile to adjacent tiles
        // upper-left, upper-right,
        // lower-left, lower-right,
        //       left, right
        List<Vector2Int> Steps = new List<Vector2Int>
        {
             new Vector2Int(0,-1), new Vector2Int(1,-1),
            new Vector2Int(-1, 1), new Vector2Int(0, 1),
            new Vector2Int(-1, 0), new Vector2Int(1, 0)
        };

        // main section (getting the adjacent tiles)
        foreach (Vector2Int step in Steps)
        {
            // getting the indices of an adjacent tile
            int x = xIndex + step.x;
            int y = yIndex + step.y;

            Vector2Int tile = new Vector2Int(y,x);

            try
            {
                // if it is inside the hex map,
                // if it was not yet determined to be an adjacent tile, and
                // **if it is not occupied by any enemy
                if (tiles.Tiles[y,x] != null && !AdjacentTiles.Contains(tile))
                    AdjacentTiles.Add(tile); // add to collection of adjacent tiles
            }
            catch(Exception){} // index out of bounds (outside of 2d array)
        }

        range--;

        // if there is more "movement" left,
        if (range > 0)
        {
            HashSet<Vector2Int> NewTiles = new HashSet<Vector2Int>();

            // search for the adjacent tiles to the determined adjacent tiles
            foreach (Vector2Int Tile in AdjacentTiles)
            {
                if(Tile != new Vector2Int(yIndex, xIndex))
                    NewTiles.UnionWith(GetAdjacentTiles(Tile.y, Tile.x, range));
            }

            // then add them to the current running list of adjacent tiles
            AdjacentTiles.UnionWith(NewTiles);
        }

        return AdjacentTiles;
    }
}
