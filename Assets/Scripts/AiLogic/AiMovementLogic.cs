using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiMovementLogic : MonoBehaviour
{
    private Transform aiTransform;
    private AiAttackLogic attackLogic;

    public TilesCreationScript Tiles;
    [SerializeField] private AspirantMovement aspirant;

    [SerializeField] private int currentYIndex;
    [SerializeField] private int currentXIndex;
    private Vector3 offset;

    private List<Vector2Int> DifferentLayerTiles;
    private List<int> RequiredExtraMovement;

    private int movementStat;
    [SerializeField] public int attackRange;
    private HashSet<Vector2Int> AvailableTiles;

    private Queue<Vector2Int> Path;
    [SerializeField] private float movementSpeed;

// CHANGED
    private PhaseHandler phaseHandler;

    [SerializeField] private bool isAvailableHighlighted; // for testing
    [SerializeField] private bool isErrorIgnored;       // to ignore error message in try-catch

    void Start()
    {
        aiTransform = GetComponent<Transform>();
        attackLogic = new AiAttackLogic();

        // position aspirant on current specified tile, with an offset to make it stand on top of it
        offset = new Vector3(0.0f, 0.22f, 0.0f); 
        aiTransform.position = Tiles.Tiles[currentYIndex,currentXIndex].transform.position + offset;

        DifferentLayerTiles = new List<Vector2Int>();
        RequiredExtraMovement = new List<int>();

        movementStat = GetComponent<PlayerObject>().movement;

        AvailableTiles = GetAdjacentTiles(currentXIndex, currentYIndex, movementStat);
        AvailableTiles.Add(new Vector2Int(currentYIndex, currentXIndex));

        if (isAvailableHighlighted)
            Tiles.Tiles[currentYIndex, currentXIndex].GetComponent<SpriteRenderer>().color = Color.yellow;

        Path = new Queue<Vector2Int>();

// CHANGED
        phaseHandler = GameObject.Find("PhaseHandler").GetComponent<PhaseHandler>();
    }

    void Update()
    {
        HashSet<Vector2Int> neighbors = GetAdjacentTiles(currentXIndex, currentYIndex, attackRange);
        Vector2Int target = new(aspirant.currentXIndex, aspirant.currentYIndex);

        
        // Call attack script first if already in range
        if (neighbors.Contains(new Vector2Int(aspirant.currentYIndex, aspirant.currentXIndex))) {
            attackLogic.attack();
        }
        if (Input.GetKeyDown(KeyCode.C)) { // Path handling
            attackLogic.canAttack = true; // Change this
            CreatePathToTarget(target, movementStat, attackRange);
        }

        if (Path.Count > 0) // Handles movement
        {
            Vector2Int nextTile = Path.Peek();
            Vector3 nextPosition = Tiles.Tiles[nextTile.y, nextTile.x].transform.position + offset;
            
            aiTransform.position = Vector3.MoveTowards(aiTransform.position, nextPosition, movementSpeed * Time.deltaTime);

            if (aiTransform.position == nextPosition)
            {
                currentYIndex = nextTile.y;
                currentXIndex = nextTile.x;
                Path.Dequeue();

                if (Path.Count == 0)
                {
                    aspirant.UpdateEnemyIndex(GetComponent<AiMovementLogic>());
// CHANGED
                    phaseHandler.enemyPositions[this.gameObject.GetComponent<PlayerObject>()] = new Vector2Int(currentYIndex, currentXIndex);
                }
            }
        }

        
    }
    HashSet<Vector2Int> GetAdjacentTiles(int xIndex, int yIndex, int range)
    {
        HashSet<Vector2Int> AdjacentTiles = new HashSet<Vector2Int>();

        for(int i = DifferentLayerTiles.Count-1; i > -1; i--)
        {
            RequiredExtraMovement[i]--;

            if (RequiredExtraMovement[i] == 0)
            {
                Vector2Int tile = DifferentLayerTiles[i];
                AdjacentTiles.Add(new Vector2Int(tile.x, tile.y));

                // indicating adjacent tiles by making them yellow
                if(isAvailableHighlighted)
                    Tiles.Tiles[tile.x,tile.y].GetComponent<SpriteRenderer>().color = Color.yellow;

                DifferentLayerTiles.RemoveAt(i);
                RequiredExtraMovement.RemoveAt(i);
            }
        }

        List<Vector2Int> Steps = new List<Vector2Int>
        {
            new Vector2Int(0, -1), new Vector2Int(0, 1),
            new Vector2Int(-1, 0), new Vector2Int(1, 0)
        };

        if (yIndex > (Tiles.returnRowCount() - 1)/2)
        {
            Steps.Add(new Vector2Int(1, -1));
            Steps.Add(new Vector2Int(-1, 1));
        }
        else if (yIndex < (Tiles.returnRowCount() - 1)/2)
        {
            Steps.Add(new Vector2Int(-1, -1));
            Steps.Add(new Vector2Int(1, 1));
        }
        else
        {
            Steps.Add(new Vector2Int(-1, -1));
            Steps.Add(new Vector2Int(-1, 1));
        }

        float currentZ = Tiles.Tiles[yIndex,xIndex].transform.position.z; // temp to determine mountain

        foreach (Vector2Int step in Steps)
        {
            int x = xIndex + step.x;
            int y = yIndex + step.y;

            try
            {
                if (Tiles.Tiles[y,x] != null && !AdjacentTiles.Contains(new Vector2Int(y,x)))
                {
                    if(Tiles.Tiles[y,x].transform.position.z == currentZ)
                    {
                        AdjacentTiles.Add(new Vector2Int(y,x));

                        if (DifferentLayerTiles.Contains(new Vector2Int(y,x)))
                        {
                            int index = DifferentLayerTiles.IndexOf(new Vector2Int(y,x));

                            DifferentLayerTiles.RemoveAt(index);
                            RequiredExtraMovement.RemoveAt(index);
                        }
                        
                        // indicating adjacent tiles by making them yellow
                        if(isAvailableHighlighted)
                            Tiles.Tiles[y,x].GetComponent<SpriteRenderer>().color = Color.yellow;
                    }

                    else if (!DifferentLayerTiles.Contains(new Vector2Int(y,x)))
                    {
                        if(Math.Abs(currentZ- Tiles.Tiles[y,x].transform.position.z) < range)
                        {
                            DifferentLayerTiles.Add(new Vector2Int(y,x));
                            RequiredExtraMovement.Add((int) Math.Abs(currentZ-Tiles.Tiles[y,x].transform.position.z));
                        }
                    }
                }
            }
            catch(Exception e)
            {
                if(!isErrorIgnored)
                    Debug.Log("Ignorable Error: " + e.Message);
            }
        }

        if (range > 1)
        {
            HashSet<Vector2Int> NewTiles = new HashSet<Vector2Int>();

            foreach (Vector2Int Tile in AdjacentTiles)
                NewTiles.UnionWith(GetAdjacentTiles(Tile.y, Tile.x, range-1));

            AdjacentTiles.UnionWith(NewTiles);
        }

        return AdjacentTiles;
    }

    void CreatePathToTarget(Vector2Int target, int movement, int range)
    {
        int currentY = currentYIndex;
        int currentX = currentXIndex;

        HashSet<Vector2Int> neighbors = GetAdjacentTiles(currentX, currentY, range);

        Vector2Int aspirantPosition = new Vector2Int(aspirant.currentYIndex, aspirant.currentXIndex);

        while (!neighbors.Contains(new Vector2Int(target.y, target.x)) && Path.Count < movement)
        {
            int stepY = 0;

            if (currentY < target.y)
                stepY = 1;
            else if (currentY > target.y)
                stepY = -1;

            int stepX = 0;

            if (currentX < target.x)
            {
                if ((stepY ==  1 && currentY < (Tiles.returnRowCount() - 1)/2) ||
                    (stepY == -1 && currentY > (Tiles.returnRowCount() - 1)/2) ||
                     stepY ==  0)
                    stepX = 1;
            }
            else if (currentX > target.x)
            {
                if ((stepY == -1 && currentY <= (Tiles.returnRowCount() - 1)/2) ||
                    (stepY ==  1 && currentY >= (Tiles.returnRowCount() - 1)/2) ||
                     stepY ==  0)
                    stepX = -1;
            }

            currentY += stepY;
            currentX += stepX;

            Path.Enqueue(new Vector2Int(currentX, currentY));
            neighbors = GetAdjacentTiles(currentX, currentY, range);
        }
    }

    public int GetYIndex()
    {
        return currentYIndex;
    }

    public int GetXIndex()
    {
        return currentXIndex;
    }
}
