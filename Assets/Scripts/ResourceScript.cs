using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceScript : MonoBehaviour
{

    public int playerManaCount;
    public int enemyManaCount;
    public int playerMaxMana;
    [SerializeField] private Slider fluxPhaseManaBar;
    [SerializeField] private Slider aspirantPhaseManaBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fluxPhaseManaBar.value = (float)playerManaCount/(float)playerMaxMana;
        aspirantPhaseManaBar.value = (float)playerManaCount/(float)playerMaxMana;
    }

    public void roundStart(int roundCount)
    {
        playerManaCount  = playerMaxMana = 10 + roundCount*10;
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
