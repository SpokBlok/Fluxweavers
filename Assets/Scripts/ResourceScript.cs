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

    public bool playerAbilityUseCheck(int manaCost)
    {
        return manaCost <= playerManaCount;
    }

    public void playerAbilityUseManaUpdate(int manaCost)
    {
        playerManaCount = playerManaCount - manaCost;
    }

    public int playerMana()
    {
        return playerManaCount; 
    }

    public bool enemyAbilityUseCheck(int manaCost)
    {
        return manaCost <= enemyManaCount;
    }

    public void enemyAbilityUseManaUpdate(int manaCost)
    {
        enemyManaCount = enemyManaCount - manaCost;
    }

    public int enemyMana()
    {
        return enemyManaCount;
    }
}
