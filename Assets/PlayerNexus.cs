using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerNexus : PlayerObject
{

    public int x;
    public int y;
    // Start is called before the first frame update
    void Start()
    {
        objectName = "Nexus";
        resourceScript = GameObject.FindObjectOfType<ResourceScript>();
        phaseHandler = GameObject.FindObjectOfType<PhaseHandler>();
        // Health Related Stuff
        health = 250;
        armor = 5;
        magicResistance = 5;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
