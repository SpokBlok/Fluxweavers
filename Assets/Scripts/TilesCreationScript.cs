using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesCreationScript : MonoBehaviour
{
    public const int rowCount = 11;
    public const int colCount = 25; // 19 + 2 * 3
    public GameObject[,] Tiles = new GameObject[rowCount, colCount];

    private HashSet<Vector2Int> AdjacentTiles;
    [SerializeField] private Color highlightColor;

    // Start is called before the first frame update
    void Start()
    {
        Transform mapTransform = this.gameObject.transform;

        // format them into a grid
        int rowNumber = -1;
        int colNumber = 0;

        for (int j = 0; j < mapTransform.childCount; j++)
        {
            Transform row = mapTransform.GetChild(j);

            if (row.gameObject.name.Equals("Left Edge") || row.gameObject.name.Equals("Right Edge"))
                break;

            rowNumber++;
            colNumber = AddLeftEdgeTiles(rowNumber, Math.Max(5-j,0));

            for (int i = row.childCount - 1; i > -1; i--)
            {
                Transform tile = row.GetChild(i);

                Tiles[rowNumber, colNumber] = tile.gameObject;

                try
                {
                    Hex hexTile = Tiles[rowNumber, colNumber].GetComponent<Hex>();
                    hexTile.y = rowNumber;
                    hexTile.x = colNumber;
                }
                catch(Exception){}

                colNumber++;
            }

            AddRightEdgeTiles(rowNumber, colNumber);
        }

        AdjacentTiles = new HashSet<Vector2Int>();

        highlightColor = Color.yellow;
    }

    int AddLeftEdgeTiles(int rowNumber, int offsetX)
    {
        if (rowNumber > 0 && rowNumber < 10)
        {
            Transform LeftEdge = GameObject.Find("Left Edge").transform;

            int colNumber = offsetX;

            if (rowNumber == 1)
            {
                Tiles[1, colNumber] = LeftEdge.GetChild(0).gameObject;

                try
                {
                    Hex hexTile = Tiles[rowNumber, colNumber].GetComponent<Hex>();
                    hexTile.y = rowNumber;
                    hexTile.x = colNumber;
                }
                catch(Exception){}

                colNumber += 1;
            }

            else if (rowNumber == 2)
            {
                Tiles[2, colNumber] = LeftEdge.GetChild(5).gameObject;

                try
                {
                    Hex hexTile = Tiles[rowNumber, colNumber].GetComponent<Hex>();
                    hexTile.y = rowNumber;
                    hexTile.x = colNumber;
                }
                catch(Exception){}
                
                colNumber += 1;
            }

            else if (rowNumber == 3)
            {
                Tiles[3, colNumber] = LeftEdge.GetChild(9).gameObject;

                try
                {
                    Hex hexTile = Tiles[rowNumber, colNumber].GetComponent<Hex>();
                    hexTile.y = rowNumber;
                    hexTile.x = colNumber;
                }
                catch(Exception){}
                
                Tiles[3, colNumber + 1] = LeftEdge.GetChild(1).gameObject;

                try
                {
                    Hex hexTile = Tiles[rowNumber, colNumber+1].GetComponent<Hex>();
                    hexTile.y = rowNumber;
                    hexTile.x = colNumber+1;
                }
                catch(Exception){}
                
                colNumber += 2;
            }

            else if (rowNumber == 4)
            {
                Tiles[4, colNumber] = LeftEdge.GetChild(12).gameObject;

                try
                {
                    Hex hexTile = Tiles[rowNumber, colNumber].GetComponent<Hex>();
                    hexTile.y = rowNumber;
                    hexTile.x = colNumber;
                }
                catch(Exception){}
                
                Tiles[4, colNumber + 1] = LeftEdge.GetChild(6).gameObject;

                try
                {
                    Hex hexTile = Tiles[rowNumber, colNumber+1].GetComponent<Hex>();
                    hexTile.y = rowNumber;
                    hexTile.x = colNumber+1;
                }
                catch(Exception){}
                
                colNumber += 2;
            }

            else if (rowNumber == 5)
            {
                Tiles[5, colNumber] = LeftEdge.GetChild(14).gameObject;

                try
                {
                    Hex hexTile = Tiles[rowNumber, colNumber].GetComponent<Hex>();
                    hexTile.y = rowNumber;
                    hexTile.x = colNumber;
                }
                catch(Exception){}
                
                Tiles[5, colNumber + 1] = LeftEdge.GetChild(10).gameObject;

                try
                {
                    Hex hexTile = Tiles[rowNumber, colNumber+1].GetComponent<Hex>();
                    hexTile.y = rowNumber;
                    hexTile.x = colNumber+1;
                }
                catch(Exception){}
                
                Tiles[5, colNumber + 2] = LeftEdge.GetChild(2).gameObject;

                try
                {
                    Hex hexTile = Tiles[rowNumber, colNumber+2].GetComponent<Hex>();
                    hexTile.y = rowNumber;
                    hexTile.x = colNumber+2;
                }
                catch(Exception){}
                
                colNumber += 3;
            }

            else if (rowNumber == 6)
            {
                Tiles[6, colNumber] = LeftEdge.GetChild(13).gameObject;

                try
                {
                    Hex hexTile = Tiles[rowNumber, colNumber].GetComponent<Hex>();
                    hexTile.y = rowNumber;
                    hexTile.x = colNumber;
                }
                catch(Exception){}
                
                Tiles[6, colNumber + 1] = LeftEdge.GetChild(7).gameObject;

                try
                {
                    Hex hexTile = Tiles[rowNumber, colNumber+1].GetComponent<Hex>();
                    hexTile.y = rowNumber;
                    hexTile.x = colNumber+1;
                }
                catch(Exception){}
                
                colNumber += 2;
            }

            else if (rowNumber == 7)
            {
                Tiles[7, colNumber] = LeftEdge.GetChild(11).gameObject;

                try
                {
                    Hex hexTile = Tiles[rowNumber, colNumber].GetComponent<Hex>();
                    hexTile.y = rowNumber;
                    hexTile.x = colNumber;
                }
                catch(Exception){}
                
                Tiles[7, colNumber + 1] = LeftEdge.GetChild(3).gameObject;

                try
                {
                    Hex hexTile = Tiles[rowNumber, colNumber+1].GetComponent<Hex>();
                    hexTile.y = rowNumber;
                    hexTile.x = colNumber+1;
                }
                catch(Exception){}
                
                colNumber += 2;
            }

            else if (rowNumber == 8)
            {
                Tiles[8, colNumber] = LeftEdge.GetChild(8).gameObject;

                try
                {
                    Hex hexTile = Tiles[rowNumber, colNumber].GetComponent<Hex>();
                    hexTile.y = rowNumber;
                    hexTile.x = colNumber;
                }
                catch(Exception){}
                
                colNumber += 1;
            }

            else if (rowNumber == 9)
            {
                Tiles[9, colNumber] = LeftEdge.GetChild(4).gameObject;

                try
                {
                    Hex hexTile = Tiles[rowNumber, colNumber].GetComponent<Hex>();
                    hexTile.y = rowNumber;
                    hexTile.x = colNumber;
                }
                catch(Exception){}
                
                colNumber += 1;
            }

            return colNumber;
        }

        return offsetX;
    }

    void AddRightEdgeTiles(int rowNumber, int colNumber)
    {
        Transform RightEdge = GameObject.Find("Right Edge").transform;

        if (rowNumber == 0)
        {
            Tiles[0, colNumber] = RightEdge.GetChild(0).gameObject;

            try
            {
                Hex hexTile = Tiles[rowNumber, colNumber].GetComponent<Hex>();
                hexTile.y = rowNumber;
                hexTile.x = colNumber;
            }
            catch(Exception){}       
        }

        else if (rowNumber == 1)
        {
            Tiles[1, colNumber] = RightEdge.GetChild(6).gameObject;

            try
            {
                Hex hexTile = Tiles[rowNumber, colNumber].GetComponent<Hex>();
                hexTile.y = rowNumber;
                hexTile.x = colNumber;
            }
            catch(Exception){}    
        }

        else if (rowNumber == 2)
        {
            Tiles[2, colNumber] = RightEdge.GetChild(1).gameObject;

            try
            {
                Hex hexTile = Tiles[rowNumber, colNumber].GetComponent<Hex>();
                hexTile.y = rowNumber;
                hexTile.x = colNumber;
            }
            catch(Exception){}    

            Tiles[2, colNumber+1] = RightEdge.GetChild(11).gameObject;

            try
            {
                Hex hexTile = Tiles[rowNumber, colNumber+1].GetComponent<Hex>();
                hexTile.y = rowNumber;
                hexTile.x = colNumber+1;
            }
            catch(Exception){}    
        }

        else if (rowNumber == 3)
        {
            Tiles[3, colNumber] = RightEdge.GetChild(7).gameObject;

            try
            {
                Hex hexTile = Tiles[rowNumber, colNumber].GetComponent<Hex>();
                hexTile.y = rowNumber;
                hexTile.x = colNumber;
            }
            catch(Exception){}    

            Tiles[3, colNumber + 1] = RightEdge.GetChild(15).gameObject;

            try
            {
                Hex hexTile = Tiles[rowNumber, colNumber+1].GetComponent<Hex>();
                hexTile.y = rowNumber;
                hexTile.x = colNumber+1;
            }
            catch(Exception){}    
        }

        else if (rowNumber == 4)
        {
            Tiles[4, colNumber] = RightEdge.GetChild(2).gameObject;

            try
            {
                Hex hexTile = Tiles[rowNumber, colNumber].GetComponent<Hex>();
                hexTile.y = rowNumber;
                hexTile.x = colNumber;
            }
            catch(Exception){}    

            Tiles[4, colNumber + 1] = RightEdge.GetChild(12).gameObject;

            try
            {
                Hex hexTile = Tiles[rowNumber, colNumber+1].GetComponent<Hex>();
                hexTile.y = rowNumber;
                hexTile.x = colNumber+1;
            }
            catch(Exception){}    

            Tiles[4, colNumber + 2] = RightEdge.GetChild(18).gameObject;

            try
            {
                Hex hexTile = Tiles[rowNumber, colNumber+2].GetComponent<Hex>();
                hexTile.y = rowNumber;
                hexTile.x = colNumber+2;
            }
            catch(Exception){}    
        }

        else if (rowNumber == 5)
        {
            Tiles[5, colNumber] = RightEdge.GetChild(8).gameObject;

            try
            {
                Hex hexTile = Tiles[rowNumber, colNumber].GetComponent<Hex>();
                hexTile.y = rowNumber;
                hexTile.x = colNumber;
            }
            catch(Exception){}    

            Tiles[5, colNumber + 1] = RightEdge.GetChild(16).gameObject;

            try
            {
                Hex hexTile = Tiles[rowNumber, colNumber+1].GetComponent<Hex>();
                hexTile.y = rowNumber;
                hexTile.x = colNumber+1;
            }
            catch(Exception){}    

            Tiles[5, colNumber + 2] = RightEdge.GetChild(20).gameObject;

            try
            {
                Hex hexTile = Tiles[rowNumber, colNumber+2].GetComponent<Hex>();
                hexTile.y = rowNumber;
                hexTile.x = colNumber+2;
            }
            catch(Exception){}    
        }

        else if (rowNumber == 6)
        {
            Tiles[6, colNumber] = RightEdge.GetChild(3).gameObject;

            try
            {
                Hex hexTile = Tiles[rowNumber, colNumber].GetComponent<Hex>();
                hexTile.y = rowNumber;
                hexTile.x = colNumber;
            }
            catch(Exception){}

            Tiles[6, colNumber + 1] = RightEdge.GetChild(13).gameObject;

            try
            {
                Hex hexTile = Tiles[rowNumber, colNumber+1].GetComponent<Hex>();
                hexTile.y = rowNumber;
                hexTile.x = colNumber+1;
            }
            catch(Exception){}    

            Tiles[6, colNumber + 2] = RightEdge.GetChild(19).gameObject;

            try
            {
                Hex hexTile = Tiles[rowNumber, colNumber+2].GetComponent<Hex>();
                hexTile.y = rowNumber;
                hexTile.x = colNumber+2;
            }
            catch(Exception){}    
        }

        else if (rowNumber == 7)
        {
            Tiles[7, colNumber] = RightEdge.GetChild(9).gameObject;

            try
            {
                Hex hexTile = Tiles[rowNumber, colNumber].GetComponent<Hex>();
                hexTile.y = rowNumber;
                hexTile.x = colNumber;
            }
            catch(Exception){}    

            Tiles[7, colNumber + 1] = RightEdge.GetChild(17).gameObject;

            try
            {
                Hex hexTile = Tiles[rowNumber, colNumber+1].GetComponent<Hex>();
                hexTile.y = rowNumber;
                hexTile.x = colNumber+1;
            }
            catch(Exception){}    
        }

        else if (rowNumber == 8)
        {
            Tiles[8, colNumber] = RightEdge.GetChild(4).gameObject;

            try
            {
                Hex hexTile = Tiles[rowNumber, colNumber].GetComponent<Hex>();
                hexTile.y = rowNumber;
                hexTile.x = colNumber;
            }
            catch(Exception){}    

            Tiles[8, colNumber+1] = RightEdge.GetChild(14).gameObject;

            try
            {
                Hex hexTile = Tiles[rowNumber, colNumber+1].GetComponent<Hex>();
                hexTile.y = rowNumber;
                hexTile.x = colNumber+1;
            }
            catch(Exception){}    
        }

        else if (rowNumber == 9)
        {
            Tiles[9, colNumber] = RightEdge.GetChild(10).gameObject;

            try
            {
                Hex hexTile = Tiles[rowNumber, colNumber].GetComponent<Hex>();
                hexTile.y = rowNumber;
                hexTile.x = colNumber;
            }
            catch(Exception){}    
        }

        else if (rowNumber == 10)
        {
            Tiles[10, colNumber] = RightEdge.GetChild(5).gameObject;

            try
            {
                Hex hexTile = Tiles[rowNumber, colNumber].GetComponent<Hex>();
                hexTile.y = rowNumber;
                hexTile.x = colNumber;
            }
            catch(Exception){}    
        }
    }

    public int returnRowCount()
    {
        return rowCount;
    }

    public int returnColCount()
    {
        return colCount;
    }

    public void SetAdjacentTiles(HashSet<Vector2Int> AdjacentTiles)
    {
        this.AdjacentTiles = AdjacentTiles;
    }

    public int GetAdjacentTilesCount()
    {
        return AdjacentTiles.Count;
    }

    public void HighlightAdjacentTiles(bool isHighlighted)
    {
        Color newColor;

        if(isHighlighted)
            newColor = highlightColor;
        else
            newColor = Color.white;

        foreach (Vector2Int tile in AdjacentTiles)
            Tiles[tile.x, tile.y].GetComponent<SpriteRenderer>().color = newColor;
    }
}
