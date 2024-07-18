using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PhasePlayerAspirant : PhaseBase
{
    PhaseBase nextState;
    private TilesCreationScript tiles; 
    private HashSet<Vector2Int> availableTiles;
    private Dictionary<Vector2Int, int> CheckedTiles;
    public String selectedAbility = "none";

    public override void EnterState(PhaseHandler ph)
    {
        nextState = ph.enemyAspirant;

        ph.stateText.text = "Player Aspirant";

        // Initialize availableTiles HashSet
        availableTiles = new HashSet<Vector2Int>();

        // Find TilesCreationScript in the scene
        tiles = GameObject.FindObjectOfType<TilesCreationScript>();
        if (tiles == null)
        {
            Debug.LogError("TilesCreationScript not found in the scene.");
        }

        // reset each player's hasMoved bool
        foreach (PlayerObject aspirant in ph.players)
        {
            aspirant.MoveCheck(aspirant);
            aspirant.hasMoved = false;
        }
    }

    public override void UpdateState(PhaseHandler ph)
    {
        if (ph.selectedPlayer != null)
        {
            AspirantMovement aspirantMovement = ph.selectedPlayer.GetComponent<AspirantMovement>();

            if (aspirantMovement == null)
                Debug.LogError("AspirantMovement script not found on PlayerObject.");
            
            // Get current position indices from AspirantMovement script
            int currentXIndex = aspirantMovement.currentXIndex;
            int currentYIndex = aspirantMovement.currentYIndex;

            if (Input.GetKeyDown(KeyCode.M) && !ph.selectedPlayer.hasMoved)
            {
                ph.alliesInRange = new HashSet<Vector2Int>();
                ph.enemiesInRange = new HashSet<Vector2Int>();

                // un-highlight the previous adjacent tiles if there are any
                if(tiles.GetAdjacentTilesCount() > 0)
                    tiles.HighlightAdjacentTiles(false);

                if (selectedAbility.Equals("Traverse"))
                    selectedAbility = "none";
                else
                {
                    selectedAbility = "Traverse";

                    int x = aspirantMovement.originalXIndex;
                    int y = aspirantMovement.originalYIndex;

                    aspirantMovement.CheckedTiles = new Dictionary<Vector2Int, int>();

                    HashSet<Vector2Int> unreachableMountains;
                    aspirantMovement.AvailableTiles = aspirantMovement.GetAdjacentTiles(x, y, ph.selectedPlayer.movement, false,
                                                                                        out unreachableMountains);

                    // un-highlight the previous adjacent tiles if there are any
                    if(tiles.GetAdjacentTilesCount() > 0)
                        tiles.HighlightAdjacentTiles(false);

                    // set the new adjacent tiles and highlight them
                    tiles.SetAdjacentTiles(aspirantMovement.AvailableTiles);
                    tiles.HighlightAdjacentTiles(true);
                }
            }
            //else if ph.selectedPlayer.actionsUsed.Contains("Traverse") {movement locked! message}

            else if (Input.GetKeyDown(KeyCode.B) && ph.rs.playerAbilityUseCheck(ph.selectedPlayer.basicAttackMana))
            {
                ph.alliesInRange = new HashSet<Vector2Int>();
                ph.enemiesInRange = new HashSet<Vector2Int>();

                // un-highlight the previous adjacent tiles if there are any
                if(tiles.GetAdjacentTilesCount() > 0)
                    tiles.HighlightAdjacentTiles(false);

                if (selectedAbility.Equals("BasicAttack"))
                    selectedAbility = "none";
                else
                {
                    selectedAbility = "BasicAttack";
                    HashSet<Vector2Int> emptyAllies = new HashSet<Vector2Int>();

                    CheckedTiles = new Dictionary<Vector2Int,int>();

                    int range = (int)ph.selectedPlayer.basicAttackRange;
                    if(range > 0)
                        range += additionalRange;

                    availableTiles = GetAdjacentTiles(ph, currentXIndex, currentYIndex, range,
                                                        out ph.enemiesInRange, out emptyAllies);

                    // set the new adjacent tiles and highlight them
                    tiles.SetAdjacentTiles(availableTiles);
                    tiles.HighlightAdjacentTiles(true);
                }
            }
                
            else if (Input.GetKeyDown(KeyCode.S) && ph.rs.playerAbilityUseCheck(ph.selectedPlayer.skillMana))
            {
                ph.alliesInRange = new HashSet<Vector2Int>();
                ph.enemiesInRange = new HashSet<Vector2Int>();

                // un-highlight the previous adjacent tiles if there are any
                if(tiles.GetAdjacentTilesCount() > 0)
                    tiles.HighlightAdjacentTiles(false);

                if (selectedAbility.Equals("Skill"))
                    selectedAbility = "none";
                else
                {
                    selectedAbility = "Skill";

                    CheckedTiles = new Dictionary<Vector2Int,int>();

                    int range = (int)ph.selectedPlayer.skillRange;
                    if(range > 0)
                        range += additionalRange;

                    if (ph.selectedPlayer.skillStatusAffectsEnemies)
                    {
                        HashSet<Vector2Int> emptyAllies = new HashSet<Vector2Int>();
                        availableTiles = GetAdjacentTiles(ph, currentXIndex, currentYIndex, range,
                                                            out ph.enemiesInRange, out emptyAllies);

                        // set the new adjacent tiles and highlight them
                        tiles.SetAdjacentTiles(availableTiles);
                        tiles.HighlightAdjacentTiles(true);
                    }

                    if (ph.selectedPlayer.skillStatusAffectsAllies)
                    {
                        HashSet<Vector2Int> emptyEnemies = new HashSet<Vector2Int>();
                        availableTiles = GetAdjacentTiles(ph, currentXIndex, currentYIndex, range,
                                                            out emptyEnemies, out ph.alliesInRange);

                        // un-highlight the previous adjacent tiles if there are any
                        if (tiles.GetAdjacentTilesCount() > 0)
                            tiles.HighlightAdjacentTiles(false);

                        // set the new adjacent tiles and highlight them
                        tiles.SetAdjacentTiles(availableTiles);
                        tiles.HighlightAdjacentTiles(true);
                    }
                }
            }

            else if (Input.GetKeyDown(KeyCode.U) && ph.rs.playerAbilityUseCheck(ph.selectedPlayer.signatureMoveMana))
            {
                ph.alliesInRange = new HashSet<Vector2Int>();
                ph.enemiesInRange = new HashSet<Vector2Int>();

                // un-highlight the previous adjacent tiles if there are any
                if(tiles.GetAdjacentTilesCount() > 0)
                    tiles.HighlightAdjacentTiles(false);

                if (selectedAbility.Equals("SignatureMove"))
                    selectedAbility = "none";
                else
                {
                    selectedAbility = "SignatureMove";

                    CheckedTiles = new Dictionary<Vector2Int,int>();

                    int range = (int)ph.selectedPlayer.signatureMoveRange;
                    if(range > 0)
                        range += additionalRange;
                    
                    if (ph.selectedPlayer.signatureMoveAffectsEnemies)
                    {
                        HashSet<Vector2Int> emptyAllies = new HashSet<Vector2Int>(); 
                        availableTiles = GetAdjacentTiles(ph, currentXIndex, currentYIndex, range,
                                                            out ph.enemiesInRange, out emptyAllies);

                        // set the new adjacent tiles and highlight them
                        tiles.SetAdjacentTiles(availableTiles);
                        tiles.HighlightAdjacentTiles(true);
                    }

                    if (ph.selectedPlayer.signatureMoveAffectsAllies)
                    {
                        HashSet<Vector2Int> emptyEnemies = new HashSet<Vector2Int>();
                        availableTiles = GetAdjacentTiles(ph, currentXIndex, currentYIndex, range,
                                                            out emptyEnemies, out ph.alliesInRange);

                        // un-highlight the previous adjacent tiles if there are any
                        if (tiles.GetAdjacentTilesCount() > 0)
                            tiles.HighlightAdjacentTiles(false);

                        // set the new adjacent tiles and highlight them
                        tiles.SetAdjacentTiles(availableTiles);
                        tiles.HighlightAdjacentTiles(true);
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (ph.selectedPlayer != null)
                ph.selectedPlayer.TogglePlayerSelection();

            ph.SwitchState(nextState);
        }
    }

    HashSet<Vector2Int> GetAdjacentTiles(PhaseHandler ph, int xIndex, int yIndex, int range, out HashSet<Vector2Int> EnemiesInRange, out HashSet<Vector2Int> AlliesInRange)
    {
        HashSet<Vector2Int> AdjacentTiles = new HashSet<Vector2Int>();
        EnemiesInRange = new HashSet<Vector2Int>();
        AlliesInRange = new HashSet<Vector2Int>();
        
        if (range == -1) // indicates that the attack / status move is global
        {
            // add all tiles
            for (int i = 0; i < tiles.returnRowCount(); i++)
            {
                for (int j = 0; j < tiles.returnColCount(); j++)
                {
                    try
                    {
                        Hex hexTile = tiles.Tiles[i, j].GetComponent<Hex>();

                        AdjacentTiles.Add(new Vector2Int(i,j));
                    }
                    catch (Exception){}
                }
            }

            // add all enemies
            foreach(Vector2Int enemy in ph.enemyPositions.Values)
            {
                EnemiesInRange.Add(enemy);
            }

            // add all allies
            foreach(Vector2Int ally in ph.playerPositions.Values)
            {
                AlliesInRange.Add(ally);
            }
            
            return AdjacentTiles;
        }

        AdjacentTiles.Add(new Vector2Int(yIndex, xIndex));

        if (range == 0)
            return AdjacentTiles;

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
                // if it is inside the hex map, and
                // if it was not yet determined to be an adjacent tile
                if (tiles.Tiles[y,x] != null && !AdjacentTiles.Contains(tile))
                {
                    bool isCheckedTile = CheckedTiles.ContainsKey(tile);
                    bool isCheckedAtLowerRange = false;

                    if(isCheckedTile)
                        isCheckedAtLowerRange = CheckedTiles[tile] < range;

                    if (!isCheckedTile || isCheckedAtLowerRange)
                    {
                        // add to collection of adjacent tiles
                        AdjacentTiles.Add(tile);

                        if (ph.enemyPositions.ContainsValue(tile))
                        {
                            EnemiesInRange.Add(tile);
                        }
                        if (ph.playerPositions.ContainsValue(tile))
                        {
                            AlliesInRange.Add(tile);
                        }
                    }
                }
            }
            catch(Exception){} // index out of bounds (outside of 2d array)
        }

        range--;

        // if there is "more movement" left,
        if (range > 0)
        {
            HashSet<Vector2Int> NewTiles = new HashSet<Vector2Int>();

            // search for the adjacent tiles to the determined adjacent tiles
            foreach (Vector2Int Tile in AdjacentTiles)
            {
                if(Tile != new Vector2Int(yIndex, xIndex))
                {
                    HashSet<Vector2Int> FurtherEnemies;
                    HashSet<Vector2Int> FurtherAllies;
                    NewTiles.UnionWith(GetAdjacentTiles(ph, Tile.y, Tile.x, range, out FurtherEnemies, out FurtherAllies));

                    EnemiesInRange.UnionWith(FurtherEnemies);
                    AlliesInRange.UnionWith(FurtherAllies);
                }
            }

            // then add them to the current running list of adjacent tiles
            AdjacentTiles.UnionWith(NewTiles);
        }

        return AdjacentTiles;
    }

    public void BasicAttackDamage(PhaseHandler ph)
    {
        float damage = 0;
        // Check for damage type
        if (ph.selectedPlayer.isBasicAttackPhysical)
            damage = ph.selectedPlayer.basicAttack(ph.selectedEnemy.armor, ph.selectedEnemy.health, ph.selectedEnemy.maxHealth);
        else
            damage = ph.selectedPlayer.basicAttack(ph.selectedEnemy.magicResistance, ph.selectedEnemy.health, ph.selectedEnemy.maxHealth);

        ph.selectedEnemy.IsAttacked(damage);

        ph.selectedEnemy = null;

        Debug.Log("Basic Attack is Used on Enemy");

        tiles.HighlightAdjacentTiles(false);
        ph.enemiesInRange = new HashSet<Vector2Int>();

        selectedAbility = "none";
    }

    public void SkillAttackDamage(PhaseHandler ph)
    {
        //for skills with damage
        if (ph.selectedPlayer.skillAttackExists)
        {
            float damage = 0;
            //Check for damage type
            if (ph.selectedPlayer.isSkillAttackPhysical)
                damage = ph.selectedPlayer.skillAttack(ph.selectedEnemy.armor);
            else
                damage = ph.selectedPlayer.skillAttack(ph.selectedEnemy.magicResistance);

            ph.selectedEnemy.IsAttacked(damage);

            Debug.Log("Skill Attack is Used on Enemy");
        }

        //for skills with buffs/debuffs
        if (ph.selectedPlayer.skillStatusExists)
        {
            HashSet<PlayerObject> targets = new HashSet<PlayerObject>();
            if (ph.selectedPlayer.skillStatusAffectsEnemies)
            {
                if (ph.selectedPlayer.skillStatusAffectsSingle)
                {
                    targets.Add(ph.selectedEnemy);
                }
                if (ph.selectedPlayer.skillStatusAffectsAOE)
                {
                    foreach (KeyValuePair<PlayerObject, Vector2Int> entry in ph.enemyPositions)
                    {
                        if (ph.enemiesInRange.Contains(entry.Value))
                        {
                            targets.Add(entry.Key);
                        }
                    }
                }
            }
            if (ph.selectedPlayer.skillStatusAffectsAllies)
            {
                if (ph.selectedPlayer.skillStatusAffectsSingle)
                {
                    // no skill exists that affects single ally atm
                }
                if (ph.selectedPlayer.skillStatusAffectsAOE)
                {
                    foreach (KeyValuePair<PlayerObject, Vector2Int> entry in ph.playerPositions)
                    {
                        if (ph.alliesInRange.Contains(entry.Value))
                        {
                            targets.Add(entry.Key);
                        }
                    }
                }
            }
            ph.selectedPlayer.skillStatus(targets);
            Debug.Log("Skill Attack Debuffed Enemy/ies");
        }

        ph.selectedEnemy = null;
        tiles.HighlightAdjacentTiles(false);
        ph.enemiesInRange = new HashSet<Vector2Int>();

        selectedAbility = "none";
    }

    public void SignatureMoveAttackDamage(PhaseHandler ph)
    {
        //for skills with damage
        if (ph.selectedPlayer.signatureMoveAttackExists)
        {
            float damage = 0;
            //Check for damage type
            if (ph.selectedPlayer.isSignatureMoveAttackPhysical)
                damage = ph.selectedPlayer.signatureMoveAttack(ph.selectedEnemy.armor);
            else
                damage = ph.selectedPlayer.signatureMoveAttack(ph.selectedEnemy.magicResistance);

            ph.selectedEnemy.IsAttacked(damage);

            Debug.Log("Signature Move Attack is Used on Enemy");
        }

        //for skills with buffs/debuffs
        if (ph.selectedPlayer.signatureMoveStatusExists)
        {
            HashSet<PlayerObject> targets = new HashSet<PlayerObject>();
            if (ph.selectedPlayer.signatureMoveAffectsEnemies)
            {
                if (ph.selectedPlayer.signatureMoveStatusAffectsSingle)
                {
                    targets.Add(ph.selectedEnemy);
                }
                if (ph.selectedPlayer.signatureMoveStatusAffectsAOE)
                {
                    foreach (KeyValuePair<PlayerObject, Vector2Int> entry in ph.enemyPositions)
                    {
                        if (ph.enemiesInRange.Contains(entry.Value))
                        {
                            targets.Add(entry.Key);
                        }
                    }
                }
            }

            if (ph.selectedPlayer.signatureMoveAffectsAllies)
            {
                if (ph.selectedPlayer.signatureMoveStatusAffectsSingle)
                {
                    // dne
                }
                if (ph.selectedPlayer.signatureMoveStatusAffectsAOE)
                {
                    foreach (KeyValuePair<PlayerObject, Vector2Int> entry in ph.playerPositions)
                    {
                        if (ph.alliesInRange.Contains(entry.Value))
                        {
                            targets.Add(entry.Key);
                        }
                    }
                }
            }
            ph.selectedPlayer.signatureMoveStatus(targets);
            Debug.Log("Signature Move Debuffed Enemies!");
        }

        ph.selectedEnemy = null;
        tiles.HighlightAdjacentTiles(false);
        ph.enemiesInRange = new HashSet<Vector2Int>();

        selectedAbility = "none";
    }
}
