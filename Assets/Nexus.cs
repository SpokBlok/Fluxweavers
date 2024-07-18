using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Nexus : PlayerObject
{
    public int x;
    public int y;

    private TilesCreationScript tiles;

    // Start is called before the first frame update
    void Start()
    {
        resourceScript = GameObject.FindObjectOfType<ResourceScript>();
        phaseHandler = GameObject.FindObjectOfType<PhaseHandler>();
        // Health Related Stuff
        health = 250;
        armor = 5;
        magicResistance = 5;

        tiles = GameObject.Find("Hextile Map").GetComponent<TilesCreationScript>();
        this.gameObject.transform.position = tiles.Tiles[y, x].transform.position + new Vector3(0.0f, 0.22f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
