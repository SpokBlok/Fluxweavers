using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void MainMenu() 
    {
        SceneManager.LoadScene("MainMenu");
    }

    public override void IsAttacked(float opponentDamage) 
    {
        if (shield == 1) 
        {
            shield = 0;
        }

        else 
        {
            health -= opponentDamage;
            SceneManager.LoadScene("LoseScreen");
        } 
    }

}
