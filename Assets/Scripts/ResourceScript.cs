using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScript : MonoBehaviour
{

    public int playerManaCount;
    public int enemyManaCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void roundStart(int roundCount)
    {
        playerManaCount = 10 + roundCount*10;
        enemyManaCount = 10 + roundCount * 10;
        if (playerManaCount > 100)
        {
            playerManaCount = 100;
            enemyManaCount = 100;
        }
    }

    public bool abilityUseCheck(int currentMana, int manaCost)
    {
        return manaCost <= currentMana;
    }

    public int abilityUseManaUpdate(int currentMana, int manaCost)
    {
        return currentMana - manaCost;
    }
}
