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

    [SerializeField] private GameObject ParentOfTiles;

    private const int rowCount = 11;
    private const int colCount = 25; // 19 + 2 * 3
    private GameObject[,] Tiles = new GameObject[rowCount,colCount];

    [SerializeField] private int currentYIndex;
    [SerializeField] private int currentXIndex;
    private Vector3 offset;

    private List<Vector2Int> DifferentLayerTiles;
    private List<int> RequiredExtraMovement;

    private int movementStat;
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

        Transform mapTransform = ParentOfTiles.transform;

        // format them into a grid
        int rowNumber = -1;
        int colNumber = 0;

        foreach (Transform row in mapTransform)
        {
            if(row.gameObject.name.Equals("Left Edge") || row.gameObject.name.Equals("Right Edge"))
                break;

            rowNumber++;
            colNumber = AddLeftEdgeTiles(rowNumber);

            for(int i = row.childCount-1; i > -1; i--)
            {
                Transform tile = row.GetChild(i);
 
                Tiles[rowNumber,colNumber] = tile.gameObject;
                colNumber++;
            }

            AddRightEdgeTiles(rowNumber, colNumber);
        }

        // position aspirant on current specified tile, with an offset to make it stand on top of it
        offset = new Vector3(0.0f, 0.22f, 0.0f); 
        aspirantTransform.position = Tiles[currentYIndex,currentXIndex].transform.position + offset;

        DifferentLayerTiles = new List<Vector2Int>();
        RequiredExtraMovement = new List<int>();

        movementStat = GetComponent<PlayerObject>().movement;

        AvailableTiles = GetAdjacentTiles(currentXIndex, currentYIndex, movementStat);
        AvailableTiles.Add(new Vector2Int(currentYIndex, currentXIndex));

        if (isAvailableHighlighted)
            Tiles[currentYIndex, currentXIndex].GetComponent<SpriteRenderer>().color = Color.yellow;

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
            Vector3 nextPosition = Tiles[nextTile.y, nextTile.x].transform.position + offset;
            
            aspirantTransform.position = Vector3.MoveTowards(aspirantTransform.position, nextPosition, movementSpeed * Time.deltaTime);

            if (aspirantTransform.position == nextPosition)
            {
                currentYIndex = nextTile.y;
                currentXIndex = nextTile.x;
                Path.Dequeue();
            }
        }

        else if (Input.GetMouseButtonDown(0))
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
            Debug.Log("Move Locked In! Make Your Next Move..");

            // changing all back to white
            if (isAvailableHighlighted)
            {
                foreach(Vector2Int Tile in AvailableTiles)
                    Tiles[Tile.x, Tile.y].GetComponent<SpriteRenderer>().color = Color.white;

                Tiles[currentYIndex, currentXIndex].GetComponent<SpriteRenderer>().color = Color.yellow;
            }

            AvailableTiles.Clear();
            AvailableTiles = GetAdjacentTiles(currentXIndex, currentYIndex, movementStat);
            AvailableTiles.Add(new Vector2Int(currentYIndex, currentXIndex));
            
            // player.hasMoved = true;
        }

        // might want some UI stuff to happen when hovering over aspirant / tile
        // else
        //     CheckHoverOverAllElements(mouseX, mouseY);
    }

    int AddLeftEdgeTiles(int rowNumber)
    {
        if (rowNumber > 0 && rowNumber < 10)
        {
            Transform LeftEdge = GameObject.Find("Left Edge").transform;

            int colNumber = 0;

            if (rowNumber == 1)
            {
                Tiles[1,0] = LeftEdge.GetChild(0).gameObject;
                colNumber = 1;
            }

            else if (rowNumber == 2)
            {
                Tiles[2,0] = LeftEdge.GetChild(5).gameObject;
                colNumber = 1;
            }

            else if (rowNumber == 3)
            {
                Tiles[3,0] = LeftEdge.GetChild(9).gameObject;
                Tiles[3,1] = LeftEdge.GetChild(1).gameObject;
                colNumber = 2;
            }

            else if (rowNumber == 4)
            {
                Tiles[4,0] = LeftEdge.GetChild(12).gameObject;
                Tiles[4,1] = LeftEdge.GetChild(6).gameObject;
                colNumber = 2;
            }

            else if (rowNumber == 5)
            {
                Tiles[5,0] = LeftEdge.GetChild(14).gameObject;
                Tiles[5,1] = LeftEdge.GetChild(10).gameObject;
                Tiles[5,2] = LeftEdge.GetChild(2).gameObject;
                colNumber = 3;
            }

            else if (rowNumber == 6)
            {
                Tiles[6,0] = LeftEdge.GetChild(13).gameObject;
                Tiles[6,1] = LeftEdge.GetChild(7).gameObject;
                colNumber = 2;
            }
            
            else if (rowNumber == 7)
            {
                Tiles[7,0] = LeftEdge.GetChild(11).gameObject;
                Tiles[7,1] = LeftEdge.GetChild(3).gameObject;
                colNumber = 2;
            }
            
            else if (rowNumber == 8)
            {
                Tiles[8,0] = LeftEdge.GetChild(8).gameObject;
                colNumber = 1;
            }

            else if (rowNumber == 9)
            {
                Tiles[9,0] = LeftEdge.GetChild(4).gameObject;
                colNumber = 1;
            }

            return colNumber;
        }

        return 0;
    }

    void AddRightEdgeTiles(int rowNumber, int colNumber)
    {
        Transform RightEdge = GameObject.Find("Right Edge").transform;

        if (rowNumber == 0)
            Tiles[0, colNumber] = RightEdge.GetChild(0).gameObject;

        else if (rowNumber == 1)
            Tiles[1,colNumber] = RightEdge.GetChild(6).gameObject;

        else if (rowNumber == 2)
        {    
            Tiles[2,colNumber] = RightEdge.GetChild(1).gameObject;
            Tiles[2,colNumber] = RightEdge.GetChild(11).gameObject;
        }

        else if (rowNumber == 3)
        {
            Tiles[3,colNumber] = RightEdge.GetChild(7).gameObject;
            Tiles[3,colNumber+1] = RightEdge.GetChild(15).gameObject;
        }

        else if (rowNumber == 4)
        {
            Tiles[4,colNumber] = RightEdge.GetChild(2).gameObject;
            Tiles[4,colNumber+1] = RightEdge.GetChild(12).gameObject;
            Tiles[4,colNumber+1] = RightEdge.GetChild(18).gameObject;
        }

        else if (rowNumber == 5)
        {
            Tiles[5,colNumber] = RightEdge.GetChild(8).gameObject;
            Tiles[5,colNumber+1] = RightEdge.GetChild(16).gameObject;
            Tiles[5,colNumber+2] = RightEdge.GetChild(20).gameObject;
        }

        else if (rowNumber == 6)
        {
            Tiles[6,colNumber] = RightEdge.GetChild(3).gameObject;
            Tiles[6,colNumber+1] = RightEdge.GetChild(13).gameObject;
            Tiles[6,colNumber+1] = RightEdge.GetChild(19).gameObject;
        }
        
        else if (rowNumber == 7)
        {
            Tiles[7,colNumber] = RightEdge.GetChild(9).gameObject;
            Tiles[7,colNumber+1] = RightEdge.GetChild(17).gameObject;
        }
        
        else if (rowNumber == 8)
        {
            Tiles[8,colNumber] = RightEdge.GetChild(4).gameObject;
            Tiles[8,colNumber] = RightEdge.GetChild(14).gameObject;
        }

        else if (rowNumber == 9)
            Tiles[9,colNumber] = RightEdge.GetChild(10).gameObject;

        else if (rowNumber == 10)
            Tiles[10,colNumber] = RightEdge.GetChild(5).gameObject;
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

            for(int i = 0; i < rowCount; i++)
            {
                for(int j = 0; j < colCount; j++)
                {
                    if (Tiles[i,j] == null)
                        continue;

                    // POSSIBLE ISSUE: since this only checks a portion of the hexagon
                    if(isMouseOnObject(mouseX, mouseY, Tiles[i,j]))
                    {
                        isHoveringOnTile = true;

                        if(AvailableTiles.Contains(new Vector2Int(i,j)))
                            Debug.Log("Aspirant can traverse on " + Tiles[i,j].name);
                        else
                            Debug.Log("Hovering over " + Tiles[i,j].name);

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
        for(int i = 0; i < rowCount; i++)
        {
            for(int j = 0; j < colCount; j++)
            {
                if (Tiles[i,j] == null)
                    continue;

                // ISSUE: since this assumes tile is rectangular but it is hexagonal
                if(isMouseOnObject(mouseX, mouseY, Tiles[i,j]))
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
                    Tiles[tile.x,tile.y].GetComponent<SpriteRenderer>().color = Color.yellow;

                DifferentLayerTiles.RemoveAt(i);
                RequiredExtraMovement.RemoveAt(i);
            }
        }

        List<Vector2Int> Steps = new List<Vector2Int>
        {
            new Vector2Int(0, -1), new Vector2Int(0, 1),
            new Vector2Int(-1, 0), new Vector2Int(1, 0)
        };

        if (yIndex > (rowCount-1)/2)
        {
            Steps.Add(new Vector2Int(1, -1));
            Steps.Add(new Vector2Int(-1, 1));
        }
        else if (yIndex < (rowCount-1)/2)
        {
            Steps.Add(new Vector2Int(-1, -1));
            Steps.Add(new Vector2Int(1, 1));
        }
        else
        {
            Steps.Add(new Vector2Int(-1, -1));
            Steps.Add(new Vector2Int(-1, 1));
        }

        float currentZ = Tiles[yIndex,xIndex].transform.position.z; // temp to determine mountain

        foreach (Vector2Int step in Steps)
        {
            int x = xIndex + step.x;
            int y = yIndex + step.y;

            try
            {
                if (Tiles[y,x] != null && !AdjacentTiles.Contains(new Vector2Int(y,x)))
                {
                    if(Tiles[y,x].transform.position.z == currentZ)
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
                            Tiles[y,x].GetComponent<SpriteRenderer>().color = Color.yellow;
                    }

                    else if (!DifferentLayerTiles.Contains(new Vector2Int(y,x)))
                    {
                        if(Math.Round(Math.Abs(currentZ-Tiles[y,x].transform.position.z)) < range)
                        {
                            DifferentLayerTiles.Add(new Vector2Int(y,x));
                            RequiredExtraMovement.Add((int) Math.Round(Math.Abs(currentZ-Tiles[y,x].transform.position.z)));
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

    Queue<Vector2Int> CreatePathToTarget(Vector2Int target)
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
                if ((stepY ==  1 && currentY < (rowCount-1)/2) ||
                    (stepY == -1 && currentY > (rowCount-1)/2) ||
                     stepY ==  0)
                    stepX = 1;
            }
            else if (currentX > target.x)
            {
                if ((stepY == -1 && currentY <= (rowCount-1)/2) ||
                    (stepY ==  1 && currentY >= (rowCount-1)/2) ||
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
