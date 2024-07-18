using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class AiMovementLogic : MonoBehaviour
{
    private Transform aiTransform;
    private AiAttackLogic attackLogic;

    public TilesCreationScript Tiles;

    [SerializeField] private int currentXIndex;
    [SerializeField] private int currentYIndex;
    private Vector3 offset;

    private int movementStat;
    [SerializeField] public int attackRange;
    private HashSet<Vector2Int> AvailableTiles;

    private Queue<Vector2Int> Path;
    [SerializeField] private float movementSpeed;

    private PhaseHandler phaseHandler;

    [SerializeField] private bool isAvailableHighlighted; // for testing
    [SerializeField] private bool isErrorIgnored;       // to ignore error message in try-catch

    void Start()
    {
        aiTransform = GetComponent<Transform>();
        attackLogic = new AiAttackLogic();

        // position aspirant on current specified tile, with an offset to make it stand on top of it
        offset = new Vector3(0.0f, 0.22f, 0.0f); 
        aiTransform.position = Tiles.Tiles[currentYIndex,currentXIndex].transform.position + offset; //  

        // movementStat = GetComponent<PlayerObject>().movement;

        AvailableTiles = GetAdjacentTiles(new(currentXIndex, currentYIndex), GetComponent<PlayerObject>().movement);
        AvailableTiles.Add(new Vector2Int(currentYIndex, currentXIndex));

        if (isAvailableHighlighted)
            Tiles.Tiles[currentYIndex, currentXIndex].GetComponent<SpriteRenderer>().color = Color.yellow;

        Path = new Queue<Vector2Int>();

        phaseHandler = GameObject.Find("PhaseHandler").GetComponent<PhaseHandler>();
    }

    void Update()
    {
        // HashSet<Vector2Int> neighbors = GetAdjacentTiles(currentXIndex, currentYIndex, attackRange);

        if (Path.Count > 0) // Handles movement
        {
            Vector2Int nextTile = Path.Peek();
            Vector3 nextPosition = Tiles.Tiles[nextTile.y, nextTile.x].transform.position + offset; //  
            
            aiTransform.position = Vector3.MoveTowards(aiTransform.position, nextPosition, movementSpeed * Time.deltaTime);

            if (aiTransform.position == nextPosition)
            {
                currentYIndex = nextTile.y;
                currentXIndex = nextTile.x;
                Path.Dequeue();
            }
        }

        if (Path.Count == 0) {
            phaseHandler.enemyPositions[gameObject.GetComponent<PlayerObject>()] = new Vector2Int(currentYIndex, currentXIndex);
            enabled = false;
        }
    }

    public void Move (Vector2Int target, Vector2Int[] enemyAi) {
        enabled = true;
        CreatePathToTarget(target, enemyAi);
    }

    public HashSet<Vector2Int> GetAdjacentTiles(int range) {
        return GetAdjacentTiles(new(currentXIndex, currentYIndex), range);
    }

    private HashSet<Vector2Int> GetAdjacentTiles(Vector2Int source, int range) {
        HashSet<Vector2Int> tileSet = new();
        int xIndex = source.x;
        int yIndex = source.y;
        // max(-N, -q-N) ≤ r ≤ min(+N, -q+N)

        for (int i = xIndex - range; i <= xIndex + range; i++) {
            for (int j = yIndex - range; j <= yIndex + range; j++) {
            // for (int j = yIndex - Math.Max(-i - range, -range); j <= yIndex + Math.Min(-i + range, range); j++) {
                if (xIndex + yIndex - range <= i + j  && i + j <= xIndex + yIndex + range)
                    tileSet.Add(new(j, i));
            }
        }

        return tileSet;
    }

    /**
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

    **/
    private Hex Vec2ToHex (Vector2Int coords) {
        try {
            return Tiles.Tiles[coords.y, coords.x].GetComponent<Hex>();
        }
        catch (Exception) {
            return Tiles.Tiles[currentYIndex, currentXIndex].GetComponent<Hex>();
        }
    }
    
    private int ManhattanDistance (Vector2Int source, Vector2Int destination) {

        try {
            Hex sourceTile = Tiles.Tiles[source.y, source.x].GetComponent<Hex>();
            Hex destinationtTile = Tiles.Tiles[destination.y, destination.x].GetComponent<Hex>();

            return  Math.Abs(destinationtTile.x - sourceTile.x) + 
                    Math.Abs(destinationtTile.y - sourceTile.y);
        }
        catch(Exception) {
            return 10_000; // Some big number so it doesn't go there
        }
        
        
        // Math.Abs(destinationtTile.layer - sourceTile.layer)) / 2;

        // return (
        //     Math.Abs(destination.x - source.x) + 
        //     Math.Abs(destination.y - source.y) // + 
        //     // Math.Abs(destination.y + destination.x - source.y - source.x)
        //     ); // 2;
    }


    void CreatePathToTarget(Vector2Int target, List<Vector2Int> nodeFilter, Vector2Int[] enemyAi) {
        Vector2Int startLocation = new(currentXIndex, currentYIndex);
        Vector2Int currentLocation = startLocation;

        Dictionary<int, Vector2Int> priorityNodes = new();
        Dictionary<Vector2Int, Vector2Int> nodeHistory = new();
        Dictionary<Vector2Int, int> costHistory = new();

        priorityNodes[ManhattanDistance(startLocation, target)] = startLocation;
        nodeHistory[startLocation] = startLocation;
        costHistory[startLocation] = 0;

        while (priorityNodes.Count > 0) {
            currentLocation = priorityNodes[priorityNodes.Keys.Min()]; // Node part of the tuple
            HashSet<Vector2Int> currentNeighbors = GetAdjacentTiles(currentLocation, attackRange);

            if (currentNeighbors.Contains(new Vector2Int(target.y, target.x))) {
                Debug.Log(currentLocation);
                // target = currentLocation;
                break;
            }

            Hex currentTile = Tiles.Tiles[currentLocation.y, currentLocation.x].GetComponent<Hex>();
            foreach (Vector2Int neighbor in GetAdjacentTiles(currentLocation, 1)) {
                Vector2Int swappedCoords = new(neighbor.y, neighbor.x);
                Hex prevTile = Tiles.Tiles[swappedCoords.y, swappedCoords.x].GetComponent<Hex>();

                int currentCost = costHistory[currentLocation] + Math.Abs(currentTile.layer - prevTile.layer) + 1;

                if (!costHistory.Keys.Contains(swappedCoords) || !enemyAi.Contains(swappedCoords) || currentCost < costHistory[swappedCoords]) {
                    int priority = ManhattanDistance(swappedCoords, target); // + currentCost
                    costHistory[swappedCoords] = currentCost;
                    priorityNodes[priority] = swappedCoords;
                    nodeHistory[swappedCoords] = currentLocation;
                }
            }
        }

        Path.Enqueue(currentLocation);
        currentLocation = nodeHistory[currentLocation]; // Start at node just before target
        // currentLocation = nodeHistory[target];

        while (!currentLocation.Equals(startLocation)) {
            Path.Enqueue(currentLocation);
            currentLocation = nodeHistory[currentLocation];
        }

        Path = new Queue<Vector2Int>(Path.Reverse());
    }

    void CreatePathToTarget(Vector2Int target, Vector2Int[] enemyAi) {

        Vector2Int startLocation = new(currentXIndex, currentYIndex);
        Vector2Int currentLocation = startLocation;

        // Queue<Vector2Int> searchSpace = new();
        Dictionary<int, Vector2Int> priorityNodes = new(); // (priority, node)
        Dictionary<Vector2Int, Vector2Int> nodeHistory = new(); // For back tracking
        // List<Vector2Int> nodefilter = new();

        priorityNodes[ManhattanDistance(startLocation, target)] = startLocation;
        // priorityNodes.Add(new Tuple<int, Vector2Int>(ManhattanDistance(startLocation, target), startLocation));
        // searchSpace.Enqueue(startLocation);

        nodeHistory[startLocation] = startLocation;
        int moveCounter = 0;

        while (priorityNodes.Count > 0) {
            
            currentLocation = priorityNodes[priorityNodes.Keys.Min()]; // Node part of the tuple
            Vector2Int prevLocation = nodeHistory[currentLocation];
            
            try {
                Hex currentTile = Tiles.Tiles[currentLocation.y, currentLocation.x].GetComponent<Hex>();
                Hex prevTile = Tiles.Tiles[prevLocation.y, prevLocation.x].GetComponent<Hex>();

                moveCounter += math.abs(currentTile.layer - prevTile.layer);
            }
            catch (Exception) {}
            

            // This is inefficient
            HashSet<Vector2Int> neighbors = GetAdjacentTiles(currentLocation, (int) GetComponent<PlayerObject>().basicAttackRange);

            // moveCounter += math.abs(currentTile.layer - prevTile.layer);

            if (moveCounter > GetComponent<PlayerObject>().movement) {
                target = nodeHistory[currentLocation];
                break;
            }

            if (neighbors.Contains(new Vector2Int(target.y, target.x))) {
                // nodeHistory[new Vector2Int(target.y, target.x)] = currentLocation;
                // target = nodeHistory[currentLocation];
                Debug.Log(currentLocation);
                target = currentLocation;
                break;
            }

            foreach (Vector2Int neighbor in GetAdjacentTiles(currentLocation, 1)) {

                Vector2Int swappedCoords = new(neighbor.y, neighbor.x);
                
                if (!nodeHistory.Keys.Contains(swappedCoords) && !enemyAi.Contains(swappedCoords)) {

                    int priority = ManhattanDistance(swappedCoords, target);
                    priorityNodes[priority] = swappedCoords;
                    nodeHistory[swappedCoords] = currentLocation;
                }

            }

            moveCounter += 1;
        }

        currentLocation = target; // Start at node just before target
        // currentLocation = nodeHistory[target];

        while (!currentLocation.Equals(startLocation)) {
            Path.Enqueue(currentLocation);
            currentLocation = nodeHistory[currentLocation];
        }

        Path = new Queue<Vector2Int>(Path.Reverse());

        // foreach (Vector2Int tile in Path) {
        //     Debug.Log(tile);
        // }
    }

    void CreatePathToTarget(Vector2Int target, int movement, int range)
    {
        int currentY = currentYIndex;
        int currentX = currentXIndex;

        HashSet<Vector2Int> neighbors = GetAdjacentTiles(new (currentX, currentY), range);

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
            neighbors = GetAdjacentTiles(new (currentX, currentY), range);
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
