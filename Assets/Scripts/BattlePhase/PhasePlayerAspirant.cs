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
            if (ph.rs.playerAbilityUseCheck(ph.player.basicAttackMana))
            {
                float damage = 0;
                // Check for damage type
                if (ph.player.isBasicAttackPhysical)
                    damage = ph.player.basicAttack(ph.enemy.armor);
                else
                    damage = ph.player.basicAttack(ph.enemy.magicResistance);

                ph.enemy.health = ph.enemy.health - damage;

                // Get current position indices from AspirantMovement script
                AspirantMovement aspirantMovement = ph.player.GetComponent<AspirantMovement>();
                if (aspirantMovement != null)
                {
                    int currentXIndex = aspirantMovement.currentXIndex;
                    int currentYIndex = aspirantMovement.currentYIndex;

                    availableTiles = aspirantMovement.GetAdjacentTiles(currentXIndex, currentYIndex, (int)ph.player.basicAttackRange);
                    availableTiles.Add(new Vector2Int(currentYIndex, currentXIndex));

                    tiles.Tiles[currentYIndex, currentXIndex].GetComponent<SpriteRenderer>().color = Color.yellow;
                }
                else
                {
                    Debug.LogError("AspirantMovement script not found on PlayerObject.");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (ph.rs.playerAbilityUseCheck(ph.player.skillMana))
            {
                // Handle skill logic
                // Placeholder: Insert your skill logic here
            }
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            if (ph.rs.playerAbilityUseCheck(ph.player.signatureMoveMana))
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
}
