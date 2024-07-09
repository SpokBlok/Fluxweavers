using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour
{
    public List<GameObject> Aspirants;
    public List<List<Vector2Int>> PathsToTile;
    public int layer = 0; // default layer is ground (0)

    // Start is called before the first frame update
    void Start()
    {
        Aspirants = new List<GameObject>();
        PathsToTile = new List<List<Vector2Int>>();

        foreach(GameObject Aspirant in GameObject.FindGameObjectsWithTag("Player"))
        {
            Aspirants.Add(Aspirant);
            PathsToTile.Add(new List<Vector2Int>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
