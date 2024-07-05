using System;
using System.Collections;
using System.Collections.Generic;
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
    private Vector3 offset;

    private List<Vector2Int> DifferentLayerTiles;
    private List<int> RequiredExtraMovement;

    private PlayerObject aspirant;
    private int movementStat;

    [SerializeField] private List<AiMovementLogic> Enemies;
    private List<Vector2Int> EnemyIndices;

    private HashSet<Vector2Int> AvailableTiles;

    private Vector2Int targetTile;
    private Queue<Vector2Int> Path;
    [SerializeField] private float movementSpeed;

    [SerializeField] private bool isAvailableHighlighted; // for testing
    [SerializeField] private bool isErrorIgnored;       // to ignore error message in try-catch

    void Start()
    {
        aspirantTransform = GetComponent<Transform>();

        isSelected = false;

        // position aspirant on current specified tile, with an offset to make it stand on top of it
        offset = new Vector3(0.0f, 0.22f, 0.0f); 
        aspirantTransform.position = Tiles.Tiles[currentYIndex,currentXIndex].transform.position + offset;

        DifferentLayerTiles = new List<Vector2Int>();
        RequiredExtraMovement = new List<int>();

        aspirant = GetComponent<PlayerObject>();
        movementStat = GetComponent<PlayerObject>().movement;

        SetUpEnemyIndices();

        AvailableTiles = GetAdjacentTiles(currentXIndex, currentYIndex, movementStat);
        AvailableTiles.Add(new Vector2Int(currentYIndex, currentXIndex));

        if (isAvailableHighlighted)
            Tiles.Tiles[currentYIndex, currentXIndex].GetComponent<SpriteRenderer>().color = Color.yellow;

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
            Vector3 nextPosition = Tiles.Tiles[nextTile.y, nextTile.x].transform.position + offset;
            
            aspirantTransform.position = Vector3.MoveTowards(aspirantTransform.position, nextPosition, movementSpeed * Time.deltaTime);

            if (aspirantTransform.position == nextPosition)
            {
                currentYIndex = nextTile.y;
                currentXIndex = nextTile.x;
                Path.Dequeue();
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
                    sr.sprite = selected;
                else
                    sr.sprite = normal;
            }
            else if(isSelected)
            {
                targetTile = GetTargetTile(mouseX, mouseY);
                Path = CreatePathToTarget(targetTile);
            }
        }

        else if (Input.GetMouseButtonDown(1)) // right click to end turn (control just for testing)
        {
            if (aspirant.hasMoved)
                Debug.Log("Make Your Next Move..");

            else
            {
                Debug.Log("Move Locked In!");

                // changing all back to white
                if (isAvailableHighlighted)
                {
                    foreach(Vector2Int Tile in AvailableTiles)
                        Tiles.Tiles[Tile.x, Tile.y].GetComponent<SpriteRenderer>().color = Color.white;

                    Tiles.Tiles[currentYIndex, currentXIndex].GetComponent<SpriteRenderer>().color = Color.yellow;
                }

                AvailableTiles.Clear();
                AvailableTiles = GetAdjacentTiles(currentXIndex, currentYIndex, movementStat);
                AvailableTiles.Add(new Vector2Int(currentYIndex, currentXIndex));
            }

            aspirant.hasMoved = !aspirant.hasMoved;
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

        Vector2Int previousEnemyPosition = new Vector2Int(EnemyIndices[index].y,EnemyIndices[index].x);

        Queue<Vector2Int> path = CreatePathToTarget(previousEnemyPosition);

        if (path.Count <= movementStat)
        {
            AvailableTiles.Add(EnemyIndices[index]);

            int x = EnemyIndices[index].x;
            int y = EnemyIndices[index].y;

            if(isAvailableHighlighted)
                Tiles.Tiles[x,y].GetComponent<SpriteRenderer>().color = Color.yellow;
        }

        int enemyY = Enemy.GetYIndex();
        int enemyX = Enemy.GetXIndex();

        EnemyIndices[index] = new Vector2Int(enemyY,enemyX);

        if (AvailableTiles.Contains(new Vector2Int(enemyY,enemyX)))
        {
            AvailableTiles.Remove(new Vector2Int(enemyY,enemyX));

            if(isAvailableHighlighted)
                Tiles.Tiles[enemyY,enemyX].GetComponent<SpriteRenderer>().color = Color.white;
        }
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
        catch(Exception e)
        {
            multiplier = 1.0f;

            if(!isErrorIgnored)
                Debug.Log("Ignorable Error: " + e.Message);
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
                        isSelected = false;
                        GetComponent<SpriteRenderer>().sprite = normal;

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

    public HashSet<Vector2Int> GetAdjacentTiles(int xIndex, int yIndex, int range)
    {
        HashSet<Vector2Int> AdjacentTiles = new HashSet<Vector2Int>();

        // accounting for tiles that were determined to be in a different layer before
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

        // setting up steps from current tile to adjacent tiles (up, down, left, right in that order)
        List<Vector2Int> Steps = new List<Vector2Int>
        {
            new Vector2Int(0, -1), new Vector2Int(0, 1),
            new Vector2Int(-1, 0), new Vector2Int(1, 0)
        };

        // setting up the other two adjacent tiles (upper-left & lower-left, or upper-right & lower-right)
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

        float currentZ = Tiles.Tiles[yIndex,xIndex].transform.position.z; // TEMP to determine mountain

        // main section (getting the adjacent tiles)
        foreach (Vector2Int step in Steps)
        {
            // getting the indices of an adjacent tile
            int x = xIndex + step.x;
            int y = yIndex + step.y;

            try
            {
                // if it is inside the hex map,
                // if it was not yet determined to be an adjacent tile, and
                // if it is not occupied by any enemy
                if (Tiles.Tiles[y,x] != null
                    && !AdjacentTiles.Contains(new Vector2Int(y,x))
                    && !EnemyIndices.Contains(new Vector2Int(y,x)))
                {
                    // if same layer
                    if(Tiles.Tiles[y,x].transform.position.z == currentZ)
                    {
                        // add to collection of adjacent tiles
                        AdjacentTiles.Add(new Vector2Int(y,x));

                        // if it was determined as a tile from a different layer before,
                        if (DifferentLayerTiles.Contains(new Vector2Int(y,x)))
                        {
                            int index = DifferentLayerTiles.IndexOf(new Vector2Int(y,x));

                            // remove it from that list
                            DifferentLayerTiles.RemoveAt(index);
                            RequiredExtraMovement.RemoveAt(index);
                        }
                        
                        // indicating adjacent tiles by making them yellow
                        if(isAvailableHighlighted)
                            Tiles.Tiles[y,x].GetComponent<SpriteRenderer>().color = Color.yellow;
                    }

                    // else (different layer)
                    // if it was not yet considered to be a tile on a different layer
                    else if (!DifferentLayerTiles.Contains(new Vector2Int(y,x)))
                    {
                        // check if it can be traversed given the "movement" left, if yes:
                        if(Math.Round(Math.Abs(currentZ- Tiles.Tiles[y,x].transform.position.z)) < range)
                        {
                            // add to collection of tiles on a different layer
                            DifferentLayerTiles.Add(new Vector2Int(y,x));
                            RequiredExtraMovement.Add((int) Math.Round(Math.Abs(currentZ- Tiles.Tiles[y,x].transform.position.z)));
                        }
                    }
                }
            }
            catch(Exception e) // index out of bounds (outside of 2d array)
            {
                if(!isErrorIgnored)
                    Debug.Log("Ignorable Error: " + e.Message);
            }
        }

        // if there is more "movement" left,
        if (range > 1)
        {
            HashSet<Vector2Int> NewTiles = new HashSet<Vector2Int>();

            // search for the adjacent tiles to the determined adjacent tiles
            foreach (Vector2Int Tile in AdjacentTiles)
                NewTiles.UnionWith(GetAdjacentTiles(Tile.y, Tile.x, range-1));

            // then add them to the current running list of adjacent tiles
            AdjacentTiles.UnionWith(NewTiles);
        }

        return AdjacentTiles;
    }

    public Queue<Vector2Int> CreatePathToTarget(Vector2Int target)
    {
        Queue<Vector2Int> Pathway = new Queue<Vector2Int>();

        int currentY = currentYIndex;
        int currentX = currentXIndex;

        while (currentY != target.y || currentX != target.x)
        {
            int stepY = 0;

            if (currentY < target.y)
                stepY = 1;
            else if (currentY > target.y)
                stepY = -1;

            int stepX = 0;

            if (currentX < target.x)
            {
                if ((stepY ==  1 && currentY < (Tiles.returnRowCount() -1)/2) ||
                    (stepY == -1 && currentY > (Tiles.returnRowCount() -1)/2) ||
                     stepY ==  0)
                    stepX = 1;
            }
            else if (currentX > target.x)
            {
                if ((stepY == -1 && currentY <= (Tiles.returnRowCount() -1)/2) ||
                    (stepY ==  1 && currentY >= (Tiles.returnRowCount() -1)/2) ||
                     stepY ==  0)
                    stepX = -1;
            }

            currentY += stepY;
            currentX += stepX;

            Pathway.Enqueue(new Vector2Int(currentX, currentY));
        }

        return Pathway;
    }

}
