using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

using FluxNamespace;

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
        List<Vector2Int> temp = new(CreatePathToTarget(target, enemyAi).Reverse());
        // Debug.Log(target + " " + temp.Count);
        // Path = new Queue<Vector2Int>(temp);
        int moveCounter = 0; // GetComponent<PlayerObject>().movement

        // Path = new Queue<Vector2Int>(temp.Take(GetComponent<PlayerObject>().movement));

        for (int i = 0; i < temp.Count - 1; i++) {

            if (moveCounter > GetComponent<PlayerObject>().movement) {
                break;
            }
            int weight = GetEdgeWeight(temp[i], temp[i+1], true);
            
            moveCounter += weight;
            Debug.Log("At " + temp[i] + " and neighbor " + temp[i+1] + ", weight is " + weight);
            Path.Enqueue(temp[i]);
        }
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

    private Hex Vec2ToHex (Vector2Int coords) {
        try {
            return Tiles.Tiles[coords.y, coords.x].GetComponent<Hex>();
        }
        catch (Exception) {
            return Tiles.Tiles[currentYIndex, currentXIndex].GetComponent<Hex>();
        }
    }
    
    private int ManhattanDistance (Vector2Int source, Vector2Int destination) {

        // try {
        //     Hex sourceTile = Tiles.Tiles[source.y, source.x].GetComponent<Hex>();
        //     Hex destinationtTile = Tiles.Tiles[destination.y, destination.x].GetComponent<Hex>();

        //     return  Math.Abs(destinationtTile.x - sourceTile.x) + 
        //             Math.Abs(destinationtTile.y - sourceTile.y);
        // }
        // catch(Exception) {
        //     return 10_000; // Some big number so it doesn't go there
        // }
        
        
        // Math.Abs(destinationtTile.layer - sourceTile.layer)) / 2;

        int dx = destination.x - source.x;
        int dy = destination.y - source.y;
        
        if (Math.Sign(dx) == Math.Sign(dy)) {
            return Math.Abs(dx + dy);
        }

        return Math.Max(Math.Abs(dx), Math.Abs(dy));

        // return (
        //      + 
        //     Math.Abs() // + 
        //     // Math.Abs(destination.y + destination.x - source.y - source.x)
        //     ); // 2;
    }

    private List<Tuple<int, Vector2Int>> Sort(List<Tuple<int, Vector2Int>> array) {
        // array.RemoveAt(0);
        return array.OrderByDescending(element => element.Item1).Reverse().ToList();
    }

    public void Test () {
        // List<Tuple<int, Vector2Int>> testSet = new(){
        //     new(10, new(1,10)),
        //     new(100, new(5,10)),
        //     new(69, new(6,9)),
        //     new(1, new(90,77)),
        //     new(0, new(98,54)),
        //     new(1000, new(30,35)),
        //     new(0, new(30,35)),
        // };

        // testSet = Sort(testSet);

        // foreach (Tuple<int, Vector2Int> t in testSet) {
        //     Debug.Log(t);
        // }

        Vector2Int target = new(16,5);
        Vector2Int[] path = CreatePathToTarget(target, new Vector2Int[]{});

        Debug.Log("Testing: " + path.Length);
        foreach (Vector2Int node in path) {
            Debug.Log(node);
        }
    }

    private int GetEdgeWeight (Vector2Int currentNode, Vector2Int nextNode, bool movement) {

        FluxNames[] mountainTerrain = new FluxNames[] {
            FluxNames.EarthArise,
            FluxNames.SeismicWave,
            FluxNames.CinderCone,
            FluxNames.Waterfall,
            FluxNames.MountainSpires,

        };

        FluxNames[] forestTerrain = new FluxNames[] {
            FluxNames.Regrowth,
            FluxNames.Reforestation,
            FluxNames.Swamp,
            FluxNames.MountainSpires,
            FluxNames.WindsweptWoods,
        };
        
        try {
            Hex currentTile = Tiles.Tiles[currentNode.x, currentNode.y].GetComponent<Hex>();
            Hex nextTile = Tiles.Tiles[nextNode.y, nextNode.x].GetComponent<Hex>();

            if (mountainTerrain.Contains(nextTile.currentFlux) ^ mountainTerrain.Contains(currentTile.currentFlux)) {
                return movement? 2:4;
            }

            if (forestTerrain.Contains(nextTile.currentFlux)) {
                return 0;
            }

            if (nextTile.currentFlux.Equals(FluxNames.None)) {
                return movement? 1:2;
            }

           // return 1;
        }
        catch (Exception) {}

        // Debug.Log("There was an error?");
        return 0;
    }

    private Vector2Int[] BacktrackPath(Dictionary<Vector2Int, Vector2Int> nodeBacktrack, Vector2Int currentLocation) {
        Vector2Int[] path = new Vector2Int[]{currentLocation};
        // Debug.Log("currentLocation: " + currentLocation);
        // foreach (Vector2Int n in nodeBacktrack.Keys) {
        //     Debug.Log(n + " " + nodeBacktrack[n]);
        // }
        
        // Debug.Log(currentLocation + " " + nodeBacktrack[currentLocation]);

        while (nodeBacktrack.Keys.Contains(currentLocation)) {
            // Debug.Log("Are you backtraccking?");
            path = path.Append(currentLocation).ToArray(); // WTF???
            currentLocation = nodeBacktrack[currentLocation];
        }
        return path;
    }

    public Vector2Int[] CreatePathToTarget(Vector2Int target, Vector2Int[] enemyAi) {
        Vector2Int startLocation = new(currentXIndex, currentYIndex);
        // Vector2Int currentLocation = startLocation;

        List<Tuple<int, Vector2Int>> frontier = new();
        Dictionary<Vector2Int, Vector2Int> nodeBacktrack = new();
        Dictionary<Vector2Int, int> nodeCost = new();

        // Minimum range should be 1 for neighbor check
        int attackRange = Math.Max((int) GetComponent<PlayerObject>().basicAttackRange, 1); 
        
        HashSet<Vector2Int> currentNeighbors = GetAdjacentTiles(startLocation, attackRange);
        if (currentNeighbors.Contains(target)) {
            // Debug.Log("Hi?");
            return new Vector2Int[]{};
        }

        frontier.Add(new(ManhattanDistance(startLocation, target), startLocation));
        nodeCost[startLocation] = 0;

        while (frontier.Count > 0) {
            // Debug.Log(frontier.Count);
            frontier = Sort(frontier);
            Vector2Int currentLocation = frontier[0].Item2;
            frontier.RemoveAt(0);

            currentNeighbors = GetAdjacentTiles(currentLocation, attackRange);
            if (currentNeighbors.Contains(new Vector2Int(target.y, target.x))) {
                // Debug.Log("Target near");
                return BacktrackPath(nodeBacktrack, currentLocation); // new(currentLocation.y, currentLocation.x)
            }

            foreach (Vector2Int neighbor in GetAdjacentTiles(currentLocation, 1)) {
                Vector2Int swappedCoords = new(neighbor.y, neighbor.x);
                // 1 should be changed to currentLocation->swappedCoords edge weight
                int currentCost = nodeCost[currentLocation] + GetEdgeWeight(currentLocation, swappedCoords, false);

                if ((!nodeCost.Keys.Contains(swappedCoords) || currentCost < nodeCost[swappedCoords]) && !enemyAi.Contains(neighbor)) {
                    nodeBacktrack[swappedCoords] = currentLocation;
                    nodeCost[swappedCoords] = currentCost;
                    frontier.Add(new(currentCost + ManhattanDistance(swappedCoords, target), swappedCoords));
                }
            }
        }

        return null;

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
