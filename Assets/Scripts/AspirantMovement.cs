using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AspirantMovement : MonoBehaviour
{
    // placeholders to see if aspirant is selected or not
    [SerializeField] private Sprite normal;
    [SerializeField] private Sprite selected;

    private Transform aspirantTransform;

    private bool isSelected;

    public TilesCreationScript Tiles;

    [SerializeField] public int currentYIndex;
    [SerializeField] public int currentXIndex;

    private int originalYIndex;
    private int originalXIndex;

    private Vector3 offset;

    private List<Vector2Int> DifferentLayerTiles;
    private List<int> RequiredExtraMovement;

    private PlayerObject aspirant;
    private int movementStat;

    private PhaseHandler phaseHandler;

    private List<AiMovementLogic> Enemies;
    private List<Vector2Int> EnemyIndices;

    private bool isMovementSkillActivated;

    private Dictionary<PlayerObject, Vector2Int> OtherAspirantIndices;

    private HashSet<Vector2Int> AvailableTiles;

    private Vector2Int targetTile;
    private Queue<Vector2Int> Path;

    [SerializeField] private float movementSpeed;

    void Start()
    {
        aspirantTransform = GetComponent<Transform>();

        isSelected = false;

        originalYIndex = currentYIndex;
        originalXIndex = currentXIndex;

        // position aspirant on current specified tile, with an offset to make it stand on top of it
        offset = new Vector3(0.0f, 0.22f, 0.0f); 
        aspirantTransform.position = Tiles.Tiles[currentYIndex,currentXIndex].transform.position + offset;

        DifferentLayerTiles = new List<Vector2Int>();
        RequiredExtraMovement = new List<int>();

        aspirant = GetComponent<PlayerObject>();
        movementStat = aspirant.movement;

        phaseHandler = GameObject.Find("PhaseHandler").GetComponent<PhaseHandler>();

        // to be updated when we get the AiMovementLogic script ready
        Enemies = new List<AiMovementLogic>();
        foreach (GameObject Enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            Enemies.Add(Enemy.GetComponent<AiMovementLogic>());

        SetUpEnemyIndices();

        isMovementSkillActivated = false;

        OtherAspirantIndices = new Dictionary<PlayerObject, Vector2Int>();
        foreach(PlayerObject player in phaseHandler.players)
        {
            AspirantMovement aspirant = player.gameObject.GetComponent<AspirantMovement>();
            
            if (aspirant != this)
            {
                int y = aspirant.currentYIndex;
                int x = aspirant.currentXIndex;

                OtherAspirantIndices[player] = new Vector2Int(y,x);
            }
        }

        HashSet<Vector2Int> unreachableMountains;
        AvailableTiles = GetAdjacentTiles(currentXIndex, currentYIndex, movementStat, out unreachableMountains);

        // target is the current tile for now
        targetTile = new Vector2Int(currentYIndex, currentXIndex);

        Path = new Queue<Vector2Int>();
    }

    void Update()
    {
        // get mouse position accdg. to transform coords
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float mouseX = mousePosition.x;
        float mouseY = mousePosition.y;

        if (Path.Count > 0)
        {
            Vector2Int nextTile = Path.Peek();
            Vector3 nextPosition = Tiles.Tiles[nextTile.x, nextTile.y].transform.position + offset;
            
            aspirantTransform.position = Vector3.MoveTowards(aspirantTransform.position, nextPosition, movementSpeed * Time.deltaTime);

            if (aspirantTransform.position == nextPosition)
            {
                currentYIndex = nextTile.x;
                currentXIndex = nextTile.y;
                Path.Dequeue();

                if (Path.Count == 0)
                {
                    phaseHandler.playerPositions[aspirant] = new Vector2Int(currentYIndex, currentXIndex);

                    foreach(PlayerObject player in phaseHandler.players)
                    {
                        if (player != this.aspirant)
                        {
                            AspirantMovement OtherAspirant = player.gameObject.GetComponent<AspirantMovement>();
                            OtherAspirant.UpdateAspirantIndex(aspirant);
                        }
                    }
                }
            }
        }

        else if (Input.GetMouseButtonDown(0) && !aspirant.hasMoved)
        {    
            if(isMouseOnObject(mouseX, mouseY, this.gameObject))
            {
                // Debug.Log("Click was on " + this.gameObject.name);
                isSelected = !isSelected;

                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                if(isSelected)
                {
                    sr.sprite = selected;

                    if(Tiles.GetAdjacentTilesCount() > 0)
                        Tiles.HighlightAdjacentTiles(false);

                    Tiles.SetAdjacentTiles(AvailableTiles);
                }
                else
                {
                    sr.sprite = normal;
                    isMovementSkillActivated = false;
                    Tiles.HighlightAdjacentTiles(false);
                }
            }
            else if(isSelected && isMovementSkillActivated)
            {
                targetTile = GetTargetTile(mouseX, mouseY);
                Path = CreatePathToTarget(targetTile);

                if (Path.Count == 0)
                {
                    GetComponent<SpriteRenderer>().sprite = normal;
                    isSelected = false;
                    isMovementSkillActivated = false;
                    Tiles.HighlightAdjacentTiles(false);
                }
            }
        }

        else if (Input.GetMouseButtonDown(1)) // right click to end turn (control just for testing)
        {
            if (aspirant.hasMoved)
                Debug.Log("Make Your Next Move..");

            else
            {
                Debug.Log("Move Locked In!");

                GetComponent<SpriteRenderer>().sprite = normal;
                isSelected = false;
                isMovementSkillActivated = false;
                Tiles.HighlightAdjacentTiles(false);

                originalXIndex = currentXIndex;
                originalYIndex = currentYIndex;

                AvailableTiles.Clear();
                HashSet<Vector2Int> unreachableMountains;
                AvailableTiles = GetAdjacentTiles(currentXIndex, currentYIndex, movementStat, out unreachableMountains);
            }

            aspirant.hasMoved = !aspirant.hasMoved;
        }

        else if (Input.GetKeyDown(KeyCode.H) && isSelected)
        {
            isMovementSkillActivated = !isMovementSkillActivated;

            if(isMovementSkillActivated)
                Tiles.HighlightAdjacentTiles(true);
            else
                Tiles.HighlightAdjacentTiles(false);
        }

        // might want some UI stuff to happen when hovering over aspirant / tile
        // else
        //     CheckHoverOverAllElements(mouseX, mouseY);
    }

    void SetUpEnemyIndices()
    {
        EnemyIndices = new List<Vector2Int>();

        foreach (AiMovementLogic Enemy in Enemies)
        {
            int y = Enemy.GetYIndex();
            int x = Enemy.GetXIndex();

            EnemyIndices.Add(new Vector2Int(y,x));
        }
    }

    public void UpdateEnemyIndex(AiMovementLogic Enemy)
    {
        int index = Enemies.IndexOf(Enemy);

        // get new enemy position
        int enemyY = Enemy.GetYIndex();
        int enemyX = Enemy.GetXIndex();

        // update running list
        EnemyIndices[index] = new Vector2Int(enemyY,enemyX);

        AvailableTiles.Clear();
        HashSet<Vector2Int> unreachableMountains;
        AvailableTiles = GetAdjacentTiles(currentXIndex, currentYIndex, movementStat, out unreachableMountains);
    }

    public void UpdateAspirantIndex(PlayerObject player)
    {
        AspirantMovement aspirant = player.gameObject.GetComponent<AspirantMovement>();
        int y = aspirant.currentYIndex;
        int x = aspirant.currentXIndex;

        OtherAspirantIndices[player] = new Vector2Int(y,x);
    }

    bool isMouseOnObject(float mouseX, float mouseY, GameObject obj)
    {
        Transform objTransform = obj.GetComponent<Transform>();

        float multiplier;

        try
        {
            if (obj.name.Substring(0,7).Equals("HexTile"))
                multiplier = 0.35f;
            else
                multiplier = 1.0f;
        }
        catch(Exception)
        {
            multiplier = 1.0f;
        }

        // get object position and dimensions
        float x = objTransform.position.x;
        float y = objTransform.position.y;
        float width = objTransform.lossyScale.x;
        float height = objTransform.lossyScale.y * multiplier;

        // check if click was on object
        if(mouseX >= x - width / 2 && mouseY >= y - height / 2 &&
            mouseX <= x + width / 2  && mouseY <= y + height / 2)
            return true;
        
        return false;
    }

    void CheckHoverOverAllElements(float mouseX, float mouseY)
    {
        if(isMouseOnObject(mouseX, mouseY, this.gameObject))
            Debug.Log("Hovering over " + this.gameObject.name);
        else
        {
            bool isHoveringOnTile = false;

            for(int i = 0; i < Tiles.returnRowCount(); i++)
            {
                for(int j = 0; j < Tiles.returnColCount(); j++)
                {
                    if (Tiles.Tiles[i,j] == null)
                        continue;

                    // POSSIBLE ISSUE: since this only checks a portion of the hexagon
                    if(isMouseOnObject(mouseX, mouseY, Tiles.Tiles[i,j]))
                    {
                        isHoveringOnTile = true;

                        if(AvailableTiles.Contains(new Vector2Int(i,j)))
                            Debug.Log("Aspirant can traverse on " + Tiles.Tiles[i,j].name);
                        else
                            Debug.Log("Hovering over " + Tiles.Tiles[i,j].name);

                        break;
                    }
                }
            }

            if (!isHoveringOnTile)
                Debug.Log("");
        }
    }

    Vector2Int GetTargetTile(float mouseX, float mouseY)
    {
        // check each tile if they were clicked on
        for(int i = 0; i < Tiles.returnRowCount(); i++)
        {
            for(int j = 0; j < Tiles.returnColCount(); j++)
            {
                if (Tiles.Tiles[i,j] == null)
                    continue;

                // ISSUE: since this assumes tile is rectangular but it is hexagonal
                if(isMouseOnObject(mouseX, mouseY, Tiles.Tiles[i,j]))
                {
                    if(AvailableTiles.Contains(new Vector2Int(i,j)))
                    {
                        // Debug.Log("Click was on " + Tiles[i,j].name);
                        return new Vector2Int(j,i);
                    }
                    else
                        break;
                }
            }
        }

        // if none were, the target tile is the current tile, which is the current position of the aspirant
        return new Vector2Int(currentXIndex, currentYIndex);
    }

    public HashSet<Vector2Int> GetAdjacentTiles(int xIndex, int yIndex, int range,
                                                out HashSet<Vector2Int> UnreachableMountains)
    {
        UnreachableMountains = new HashSet<Vector2Int>();

        HashSet<Vector2Int> AdjacentTiles = new HashSet<Vector2Int>();
        AdjacentTiles.Add(new Vector2Int(currentYIndex, currentXIndex));

        // accounting for tiles that were determined to be in a different layer before
        for(int i = DifferentLayerTiles.Count-1; i > -1; i--)
        {
            RequiredExtraMovement[i]--;

            if (RequiredExtraMovement[i] == 0)
            {
                Vector2Int tile = DifferentLayerTiles[i];
                AdjacentTiles.Add(tile);

                DifferentLayerTiles.RemoveAt(i);
                RequiredExtraMovement.RemoveAt(i);
            }
        }

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

        // get current layer of current tile
        int currentLayer = Tiles.Tiles[yIndex,xIndex].GetComponent<Hex>().layer;

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
                // if it was not yet determined to be an adjacent tile, 
                // if it is not occupied by any other aspirant, and
                // if it is not occupied by any enemy
                if (Tiles.Tiles[y,x] != null
                    && !AdjacentTiles.Contains(tile)
                    && !OtherAspirantIndices.ContainsValue(tile)
                    && !EnemyIndices.Contains(tile))
                {
                    int tileLayer = Tiles.Tiles[y,x].GetComponent<Hex>().layer;

                    // if same layer
                    if(tileLayer == currentLayer)
                    {
                        // add to collection of adjacent tiles
                        AdjacentTiles.Add(tile);

                        // if it was determined as a tile from a different layer before,
                        if (DifferentLayerTiles.Contains(tile))
                        {
                            int i = DifferentLayerTiles.IndexOf(tile);

                            // remove it from that list
                            DifferentLayerTiles.RemoveAt(i);
                            RequiredExtraMovement.RemoveAt(i);
                        }
                    }

                    // else (different layer)
                    // if it was not yet considered to be a tile on a different layer
                    else if (!DifferentLayerTiles.Contains(tile))
                    {
                        // check if it can be traversed given the "movement" left, if yes:
                        if(Math.Abs(currentLayer-tileLayer) < range)
                        {
                            // add to collection of tiles on a different layer
                            DifferentLayerTiles.Add(tile);
                            RequiredExtraMovement.Add((int) Math.Abs(currentLayer-tileLayer));
                        }
                        else
                            UnreachableMountains.Add(tile);
                    }
                }
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
                if(Tile != new Vector2Int(currentYIndex, currentXIndex) ||
                    Tile != new Vector2Int(yIndex, xIndex))
                {
                    HashSet<Vector2Int> NewMountains;
                    NewTiles.UnionWith(GetAdjacentTiles(Tile.y, Tile.x, range, out NewMountains));

                    UnreachableMountains.UnionWith(NewMountains);
                }
            }

            // then add them to the current running list of adjacent tiles
            AdjacentTiles.UnionWith(NewTiles);
        }

        return AdjacentTiles;
    }

    int ManhattanDistance (Vector2Int source, Vector2Int destination)
    {
        return (
                Math.Abs(destination.x - source.x) + 
                Math.Abs(destination.y - source.y)
            );
    }

    Queue<Vector2Int> CreatePathToTarget(Vector2Int target)
    {
        Queue<Vector2Int> Pathway = new Queue<Vector2Int>();

        if (target.x == currentXIndex && target.y == currentYIndex)
        {
            Pathway.Enqueue(new Vector2Int(currentYIndex, currentXIndex));
            return Pathway;
        }

        Vector2Int startLocation = new Vector2Int(currentXIndex, currentYIndex);
        Vector2Int currentLocation = new Vector2Int(currentXIndex, currentYIndex);

        Dictionary<int, Vector2Int> priorityNodes = new(); // (priority, node)
        Dictionary<Vector2Int, Vector2Int> nodeHistory= new(); // For back tracking
        
        priorityNodes[ManhattanDistance(startLocation, target)] = startLocation;

        nodeHistory[startLocation] = startLocation;

        while (priorityNodes.Count > 0)
        {
            currentLocation = priorityNodes[priorityNodes.Keys.Min()]; // Node part of the tuple

            if (currentLocation.x == target.x && currentLocation.y == target.y)
                break;

            HashSet<Vector2Int> unreachableMountains;
            foreach (Vector2Int neighbor in GetAdjacentTiles(currentLocation.x, currentLocation.y, 1, out unreachableMountains))
            {
                Vector2Int swappedCoords = new(neighbor.y, neighbor.x);
                
                if (!nodeHistory.Keys.Contains(swappedCoords) && neighbor.x >= 0 && neighbor.y >= 0)
                {
                    int priority = ManhattanDistance(swappedCoords, target);
                    priorityNodes[priority] = swappedCoords;
                    nodeHistory[swappedCoords] = currentLocation;
                }
            }

            Hex currentTile = Tiles.Tiles[currentLocation.y, currentLocation.x].GetComponent<Hex>();

            foreach(Vector2Int mountain in unreachableMountains)
            {
                Vector2Int swappedCoords = new(mountain.y, mountain.x);
                
                if (!nodeHistory.Keys.Contains(swappedCoords) && mountain.x >= 0 && mountain.y >= 0)
                {
                    int priority = ManhattanDistance(swappedCoords, target);

                    Hex nextTile = Tiles.Tiles[swappedCoords.y, swappedCoords.x].GetComponent<Hex>();
                    int layerDifference = (int) Math.Abs(currentTile.layer - nextTile.layer);
                    priority += layerDifference;

                    priorityNodes[priority] = swappedCoords;
                    nodeHistory[swappedCoords] = currentLocation;
                }
            }
        }

        while (!currentLocation.Equals(startLocation))
        {
            Pathway.Enqueue(new Vector2Int(currentLocation.y, currentLocation.x));
            currentLocation = nodeHistory[currentLocation];
        }

        Pathway = new Queue<Vector2Int>(Pathway.Reverse());

        return Pathway;

        // foreach (Vector2Int tile in Path) {
        //     Debug.Log(tile);
        // }
    }
}
