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

    private const int rowCount = 3;
    private const int colCount = 4;
    private GameObject[,] Tiles = new GameObject[rowCount,colCount];

    [SerializeField] private int currentYIndex;
    [SerializeField] private int currentXIndex;

    private Vector2Int targetTile;
    private Queue<Vector2Int> Path;
    [SerializeField] private float movementSpeed;

    void Start()
    {
        aspirantTransform = GetComponent<Transform>();

        isSelected = false;

        // sample tiles
        // to be changed with only tiles accessible to the aspirant
        Transform mapTransform = GameObject.Find("SampleMap").transform;

        // format them into a grid
        int rowNumber = -1;
        int colNumber = 0;
        float previousY = 99.9f;

        foreach (Transform child in mapTransform)
        {
            if (child.position.y != previousY)
            {
                previousY = child.position.y;
                rowNumber++;
                colNumber = 0;
            }

            Tiles[rowNumber,colNumber] = child.gameObject;
            colNumber += 1;
        }

        // position aspirant on current specified tile
        aspirantTransform.position = Tiles[currentYIndex,currentXIndex].transform.position;

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
            Vector3 nextPosition = Tiles[nextTile.y, nextTile.x].transform.position;
            
            aspirantTransform.position = Vector3.MoveTowards(aspirantTransform.position, nextPosition, movementSpeed * Time.deltaTime);

            if (aspirantTransform.position == nextPosition)
            {
                currentYIndex = nextTile.y;
                currentXIndex = nextTile.x;
                Path.Dequeue();

                // if (Path.Count == 0)
                    //player.hasMoved = true;
            }
        }

        else if (Input.GetMouseButtonDown(0))
        {    
            if(isMouseOnObject(mouseX, mouseY, this.gameObject))
            {
                Debug.Log("Click was on " + this.gameObject.name);
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
                CreatePathToTarget(targetTile);
            }
        }

        // might want some UI stuff to happen when hovering over aspirant / tile
        // else
        //     CheckHoverOverAllElements(mouseX, mouseY);
    }

    bool isMouseOnObject(float mouseX, float mouseY, GameObject obj)
    {
        Transform objTransform = obj.GetComponent<Transform>();

        // get object position and dimensions
        float x = objTransform.position.x;
        float y = objTransform.position.y;
        float width = objTransform.lossyScale.x;
        float height = objTransform.lossyScale.y;

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

                    // ISSUE: since this assumes tile is rectangular but it is hexagonal
                    if(isMouseOnObject(mouseX, mouseY, Tiles[i,j]))
                    {
                        Debug.Log("Hovering over " + Tiles[i,j].name);
                        isHoveringOnTile = true;
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
                    isSelected = false;
                    GetComponent<SpriteRenderer>().sprite = normal;

                    Debug.Log("Click was on " + Tiles[i,j].name);
                    return new Vector2Int(j,i);
                }
            }
        }

        // if none were, the target tile is the current tile, which is the current position of the aspirant
        return new Vector2Int(currentYIndex, currentXIndex);
    }

    List<Vector2Int> GetAdjacentTiles(int xIndex, int yIndex)
    {
        List<Vector2Int> AdjacentTiles = new List<Vector2Int>();

        return AdjacentTiles;
    }

    void CreatePathToTarget(Vector2Int target)
    {
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

            Path.Enqueue(new Vector2Int(currentX, currentY));
        }
    }

}
