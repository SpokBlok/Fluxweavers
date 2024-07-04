using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesCreationScript : MonoBehaviour
{

    [SerializeField] private GameObject ParentOfTiles;

    public const int rowCount = 11;
    public const int colCount = 25; // 19 + 2 * 3
    public GameObject[,] Tiles = new GameObject[rowCount, colCount];

    // Start is called before the first frame update
    void Start()
    {
        Transform mapTransform = ParentOfTiles.transform;

        // format them into a grid
        int rowNumber = -1;
        int colNumber = 0;

        foreach (Transform row in mapTransform)
        {
            if (row.gameObject.name.Equals("Left Edge") || row.gameObject.name.Equals("Right Edge"))
                break;

            rowNumber++;
            colNumber = AddLeftEdgeTiles(rowNumber);

            for (int i = row.childCount - 1; i > -1; i--)
            {
                Transform tile = row.GetChild(i);

                Tiles[rowNumber, colNumber] = tile.gameObject;
                colNumber++;
            }

            AddRightEdgeTiles(rowNumber, colNumber);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int AddLeftEdgeTiles(int rowNumber)
    {
        if (rowNumber > 0 && rowNumber < 10)
        {
            Transform LeftEdge = GameObject.Find("Left Edge").transform;

            int colNumber = 0;

            if (rowNumber == 1)
            {
                Tiles[1, 0] = LeftEdge.GetChild(0).gameObject;
                colNumber = 1;
            }

            else if (rowNumber == 2)
            {
                Tiles[2, 0] = LeftEdge.GetChild(5).gameObject;
                colNumber = 1;
            }

            else if (rowNumber == 3)
            {
                Tiles[3, 0] = LeftEdge.GetChild(9).gameObject;
                Tiles[3, 1] = LeftEdge.GetChild(1).gameObject;
                colNumber = 2;
            }

            else if (rowNumber == 4)
            {
                Tiles[4, 0] = LeftEdge.GetChild(12).gameObject;
                Tiles[4, 1] = LeftEdge.GetChild(6).gameObject;
                colNumber = 2;
            }

            else if (rowNumber == 5)
            {
                Tiles[5, 0] = LeftEdge.GetChild(14).gameObject;
                Tiles[5, 1] = LeftEdge.GetChild(10).gameObject;
                Tiles[5, 2] = LeftEdge.GetChild(2).gameObject;
                colNumber = 3;
            }

            else if (rowNumber == 6)
            {
                Tiles[6, 0] = LeftEdge.GetChild(13).gameObject;
                Tiles[6, 1] = LeftEdge.GetChild(7).gameObject;
                colNumber = 2;
            }

            else if (rowNumber == 7)
            {
                Tiles[7, 0] = LeftEdge.GetChild(11).gameObject;
                Tiles[7, 1] = LeftEdge.GetChild(3).gameObject;
                colNumber = 2;
            }

            else if (rowNumber == 8)
            {
                Tiles[8, 0] = LeftEdge.GetChild(8).gameObject;
                colNumber = 1;
            }

            else if (rowNumber == 9)
            {
                Tiles[9, 0] = LeftEdge.GetChild(4).gameObject;
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
            Tiles[1, colNumber] = RightEdge.GetChild(6).gameObject;

        else if (rowNumber == 2)
        {
            Tiles[2, colNumber] = RightEdge.GetChild(1).gameObject;
            Tiles[2, colNumber] = RightEdge.GetChild(11).gameObject;
        }

        else if (rowNumber == 3)
        {
            Tiles[3, colNumber] = RightEdge.GetChild(7).gameObject;
            Tiles[3, colNumber + 1] = RightEdge.GetChild(15).gameObject;
        }

        else if (rowNumber == 4)
        {
            Tiles[4, colNumber] = RightEdge.GetChild(2).gameObject;
            Tiles[4, colNumber + 1] = RightEdge.GetChild(12).gameObject;
            Tiles[4, colNumber + 1] = RightEdge.GetChild(18).gameObject;
        }

        else if (rowNumber == 5)
        {
            Tiles[5, colNumber] = RightEdge.GetChild(8).gameObject;
            Tiles[5, colNumber + 1] = RightEdge.GetChild(16).gameObject;
            Tiles[5, colNumber + 2] = RightEdge.GetChild(20).gameObject;
        }

        else if (rowNumber == 6)
        {
            Tiles[6, colNumber] = RightEdge.GetChild(3).gameObject;
            Tiles[6, colNumber + 1] = RightEdge.GetChild(13).gameObject;
            Tiles[6, colNumber + 1] = RightEdge.GetChild(19).gameObject;
        }

        else if (rowNumber == 7)
        {
            Tiles[7, colNumber] = RightEdge.GetChild(9).gameObject;
            Tiles[7, colNumber + 1] = RightEdge.GetChild(17).gameObject;
        }

        else if (rowNumber == 8)
        {
            Tiles[8, colNumber] = RightEdge.GetChild(4).gameObject;
            Tiles[8, colNumber] = RightEdge.GetChild(14).gameObject;
        }

        else if (rowNumber == 9)
            Tiles[9, colNumber] = RightEdge.GetChild(10).gameObject;

        else if (rowNumber == 10)
            Tiles[10, colNumber] = RightEdge.GetChild(5).gameObject;
    }

    public int returnRowCount()
    {
        return rowCount;
    }

    public int returnColCount()
    {
        return colCount;
    }
}
